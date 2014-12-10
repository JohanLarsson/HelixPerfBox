namespace HelixPerfBox
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media.Media3D;

    public class ItemContainerGenerator3D
    {
        private readonly WeakReference _parent = new WeakReference(null);
        private readonly ConcurrentQueue<Visual3D> _cache = new ConcurrentQueue<Visual3D>();
        private readonly ConditionalWeakTable<object, Visual3D> _map = new ConditionalWeakTable<object, Visual3D>();

        public ItemContainerGenerator3D(ItemsControl3D parent)
        {
            this._parent.Target = parent;
            CollectionChangedEventManager.AddHandler(parent.Items, this.OnCollectionChanged);
        }

        public ItemsControl3D Parent
        {
            get
            {
                return (ItemsControl3D)this._parent.Target;
            }
        }

        public Visual3D GetContainerOrDefaultForItem(object item)
        {
            Visual3D container;
            if (!TryGetContainerForItem(item, out container))
            {
                return null;
            }
            return container;
        }

        public Visual3D GetContainerForItem(object item)
        {
            Visual3D container;
            if (!TryGetContainerForItem(item, out container))
            {
                throw new InvalidOperationException("Could not find container for item");
            }
            return container;
        }

        public bool TryGetContainerForItem(object item, out Visual3D visual)
        {
            if (!_map.TryGetValue(item, out visual))
            {
                return false;
            }
            return true;
        }

        public virtual void Refresh()
        {
            this.Reset(this.Parent.Items);
        }

        protected virtual void Reset(IEnumerable newItems)
        {
            RemoveAll();
            Add(this.Parent.Items);
        }

        protected virtual Visual3D GenerateNext()
        {
            bool temp;
            return GenerateNext(out temp);
        }

        protected virtual Visual3D GenerateNext(out bool isNewlyRealized)
        {
            Visual3D visual3D;
            isNewlyRealized = false;
            if (!_cache.TryDequeue(out visual3D))
            {
                visual3D = CreateNewContainer();
                isNewlyRealized = true;
            }
            return visual3D;
        }

        protected virtual void RemoveAll()
        {
            var start = this.Parent.Children.Count - 1;
            for (int i = start; i >= 0; i--)
            {
                var child = this.Parent.Children[i];
                this.Remove(child);
            }
        }

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

        protected virtual void Remove(Visual3D container)
        {
            this.Parent.Children.Remove(container);
            UnlinkContainerFromItem(container, this.Parent);
            Recycle(container);
        }

        public void Add(IEnumerable newItems)
        {
            if (newItems == null)
            {
                return;
            }
            foreach (var newItem in newItems)
            {
                var container = GenerateNext();
                LinkContainerForItem(container, newItem, this.Parent);
                this.Parent.Children.Add(container);
                System.Windows.Threading.Dispatcher.Yield();
            }
        }

        protected virtual void Recycle(Visual3D visual3D)
        {
            _cache.Enqueue(visual3D);
        }

        protected virtual void UnlinkContainerFromItem(Visual3D container, ItemsControl3D host)
        {
            var item = container.GetValue(ItemContainer3D.ItemProperty);
            container.ClearValue(ItemContainer3D.ItemProperty);
            if (container == item)
                return;
            _map.Remove(item);
            DependencyProperty dp = FrameworkElement.DataContextProperty;
            container.SetValue(dp, null);
        }

        protected virtual void LinkContainerForItem(Visual3D container, object item, ItemsControl3D host)
        {
            container.SetValue(ItemContainer3D.ItemProperty, item);
            if (container == item)
                return;
            _map.Add(item, container);
            host.ItemTemplate.SetBindings(container, item);
            DependencyProperty dp = FrameworkElement.DataContextProperty;
            container.SetValue(dp, item);
        }

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
                    this.Remove(e.OldItems);
                    this.Add(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    return;
                case NotifyCollectionChangedAction.Reset:
                    this.Reset(this.Parent.Items);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        protected virtual Visual3D CreateNewContainer()
        {
            return this.Parent.ItemTemplate.Create();
        }
    }
}
