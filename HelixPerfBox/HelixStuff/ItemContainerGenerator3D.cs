// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemContainerGenerator3D.cs" company="">
//   
// </copyright>
// <summary>
//   The item container generator 3 d.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media.Media3D;
    using System.Windows.Threading;

    [Obsolete("Can be cleaned up, started implementing IItemContainerGenerator hence the API")]
    /// <summary>
    /// The item container generator 3 d.
    /// </summary>
    public class ItemContainerGenerator3D
    {
        /// <summary>
        /// The _parent.
        /// </summary>
        private readonly WeakReference _parent = new WeakReference(null);

        /// <summary>
        /// The _cache.
        /// </summary>
        private readonly ConcurrentQueue<Visual3D> _cache = new ConcurrentQueue<Visual3D>();

        /// <summary>
        /// The _map.
        /// </summary>
        private readonly ConditionalWeakTable<object, Visual3D> _map = new ConditionalWeakTable<object, Visual3D>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainerGenerator3D"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        public ItemContainerGenerator3D(ItemsControl3D parent)
        {
            _parent.Target = parent;
            CollectionChangedEventManager.AddHandler(parent.Items, OnCollectionChanged);
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        public ItemsControl3D Parent
        {
            get
            {
                return _parent.Target as ItemsControl3D;
            }
        }

        /// <summary>
        /// The get container or default for item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="Visual3D"/>.
        /// </returns>
        public Visual3D GetContainerOrDefaultForItem(object item)
        {
            Visual3D container;
            if (!TryGetContainerForItem(item, out container))
            {
                return null;
            }

            return container;
        }

        /// <summary>
        /// The get container for item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="Visual3D"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public Visual3D GetContainerForItem(object item)
        {
            Visual3D container;
            if (!TryGetContainerForItem(item, out container))
            {
                throw new InvalidOperationException("Could not find container for item");
            }

            return container;
        }

        /// <summary>
        /// The try get container for item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="visual">
        /// The visual.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool TryGetContainerForItem(object item, out Visual3D visual)
        {
            if (!_map.TryGetValue(item, out visual))
            {
                visual = item as Visual3D; // Not wrapped
                if (visual != null)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        public virtual void OnItemTemplateChanged(TemplateModel oldItemTemplate, TemplateModel newItemTemplate)
        {
            RemoveAll();
            ClearCache();
            Add(Parent.Items);
        }

        /// <summary>
        /// The reset.
        /// This clears the cache and generates new containers.
        /// Call when template changes
        /// </summary>
        /// <param name="newItems">
        /// The new items.
        /// </param>
        protected virtual void Reset(IEnumerable newItems)
        {
            RemoveAll();
            Add(Parent.Items);
        }

        protected virtual void ClearCache()
        {
            Visual3D container;
            while (_cache.TryDequeue(out container))
            {
            }
        }

        /// <summary>
        /// The generate next.
        /// </summary>
        /// <returns>
        /// The <see cref="Visual3D"/>.
        /// </returns>
        protected virtual Visual3D GenerateNext(object item)
        {
            bool temp;
            return GenerateNext(item, out temp);
        }

        /// <summary>
        /// The generate next.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isNewlyRealized">
        /// The is newly realized.
        /// </param>
        /// <returns>
        /// The <see cref="Visual3D"/>.
        /// </returns>
        protected virtual Visual3D GenerateNext(object item, out bool isNewlyRealized)
        {
            Visual3D visual3D;
            isNewlyRealized = false;
            if (!_cache.TryDequeue(out visual3D))
            {
                visual3D = CreateNewContainer(item);
                isNewlyRealized = visual3D != item;
            }

            return visual3D;
        }

        /// <summary>
        /// The remove all.
        /// </summary>
        protected virtual void RemoveAll()
        {
            var start = Parent.Children.Count - 1;
            for (int i = start; i >= 0; i--)
            {
                var child = Parent.Children[i];
                Remove(child);
            }
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="oldItems">
        /// The old items.
        /// </param>
        protected virtual void Remove(IEnumerable oldItems)
        {
            if (oldItems == null)
            {
                return;
            }

            foreach (var remove in oldItems)
            {
                var container = GetContainerForItem(remove);
                Remove(container);
            }
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        protected virtual void Remove(Visual3D container)
        {
            Parent.Children.Remove(container);
            UnlinkContainerFromItem(container);
            Recycle(container);
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="newItems">
        /// The new items.
        /// </param>
        protected virtual void Add(IEnumerable newItems)
        {
            if (newItems == null)
            {
                return;
            }

            foreach (var newItem in newItems)
            {
                var container = GenerateNext(newItem);
                LinkContainerForItem(container, newItem);
                if (!ReferenceEquals(container, newItem) && Parent.ItemTemplate != null)
                {
                    ApplyTemplate(container, newItem);
                }
                Parent.Children.Add(container);
                Dispatcher.Yield();
            }
        }

        protected virtual void ApplyTemplate(Visual3D container, object item)
        {
            Parent.ItemTemplate.Rebind(container, item);
        }

        /// <summary>
        /// The recycle.
        /// </summary>
        /// <param name="visual3D">
        /// The visual 3 d.
        /// </param>
        protected virtual void Recycle(Visual3D visual3D)
        {
            _cache.Enqueue(visual3D);
        }

        /// <summary>
        /// The unlink container from item.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="host">
        /// The host.
        /// </param>
        protected virtual void UnlinkContainerFromItem(Visual3D container)
        {
            var item = container.GetValue(ItemContainer3D.ItemProperty);
            container.ClearValue(ItemContainer3D.ItemProperty);
            //container.ClearValue(FreezableExt.DataContextProxyProperty);
            if (item == null)
                return;
            _map.Remove(item);
        }

        /// <summary>
        /// The link container for item.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="host">
        /// The host.
        /// </param>
        protected virtual void LinkContainerForItem(Visual3D container, object item)
        {
            if (container == item)
                return;
            container.SetValue(ItemContainer3D.ItemProperty, item);
            _map.Add(item, container);
        }

        /// <summary>
        /// The on collection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Add(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Remove(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Remove(e.OldItems);
                    Add(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    return;
                case NotifyCollectionChangedAction.Reset:
                    Reset(Parent.Items);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// The create new container.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>
        /// The <see cref="Visual3D"/>.
        /// </returns>
        protected virtual Visual3D CreateNewContainer(object item)
        {
            if (item is Visual3D)
            {
                return item as Visual3D;
            }
            return Parent.ItemTemplate.Create();
        }
    }
}
