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
        private readonly ItemsControl3D _itemsControl3D;
        private readonly ConcurrentQueue<Visual3D> _cache = new ConcurrentQueue<Visual3D>();
        private readonly ConditionalWeakTable<object, Visual3D> _map = new ConditionalWeakTable<object, Visual3D>();

        public static readonly DependencyProperty ItemContainerForItemProperty = DependencyProperty.RegisterAttached(
            "ItemContainerForItem",
            typeof(object),
            typeof(ItemContainerGenerator3D),
            new PropertyMetadata(default(object)));


        public ItemContainerGenerator3D(ItemsControl3D itemsControl3D)
        {
            _itemsControl3D = itemsControl3D;
            CollectionChangedEventManager.AddHandler(itemsControl3D.Items, this.OnCollectionChanged);
        }

        public static void SetItemContainerForItem(Visual3D element, object value)
        {
            element.SetValue(ItemContainerForItemProperty, value);
        }

        public static object GetItemContainerForItem(DependencyObject element)
        {
            return (object)element.GetValue(ItemContainerForItemProperty);
        }

        public void Refresh()
        {
            this.Reset(_itemsControl3D.Items);
        }

        public void Reset(IEnumerable newItems)
        {
            RemoveAll();
            Add(_itemsControl3D.Items);
        }

        public Visual3D GenerateNext()
        {
            bool temp;
            return GenerateNext(out temp);
        }

        public virtual Visual3D GenerateNext(out bool isNewlyRealized)
        {
            Visual3D visual3D;
            isNewlyRealized = false;
            if (!_cache.TryDequeue(out visual3D))
            {
                visual3D = _itemsControl3D.ItemTemplate.Create();
                isNewlyRealized = true;
            }
            return visual3D;
        }

        public void PrepareItemContainer(Visual3D container)
        {
            var modelVisual3D = _itemsControl3D.ItemTemplate.Create();
            _cache.Enqueue(modelVisual3D);
        }

        public void RemoveAll()
        {
            var start = this._itemsControl3D.Children.Count - 1;
            for (int i = start; i >= 0; i--)
            {
                var child = this._itemsControl3D.Children[i];
                this.Remove(child);
            }
        }

        public void Remove(IEnumerable oldItems)
        {
            if (oldItems == null)
            {
                return;
            }
            foreach (var remove in oldItems)
            {
                Visual3D container;
                if (!_map.TryGetValue(remove, out container))
                {
                    throw new InvalidOperationException("Could not find container for item");
                }
                Remove(container);
            }
        }

        private void Remove(Visual3D container)
        {
            _itemsControl3D.Children.Remove(container);
            UnlinkContainerFromItem(container, _itemsControl3D);
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
                LinkContainerForItem(container, newItem, _itemsControl3D);
                _itemsControl3D.Children.Add(container);
                System.Windows.Threading.Dispatcher.Yield();
            }
        }

        public void Recycle(Visual3D visual3D)
        {
            _cache.Enqueue(visual3D);
        }

        internal void UnlinkContainerFromItem(Visual3D container, ItemsControl3D host)
        {
            var item = GetItemContainerForItem(container);
            container.ClearValue(ItemContainerForItemProperty);
            if (container == item)
                return;
            _map.Remove(item);
            DependencyProperty dp = FrameworkElement.DataContextProperty;
            container.SetValue(dp, null);
        }

        internal void LinkContainerForItem(Visual3D container, object item, ItemsControl3D host)
        {
            container.SetValue(ItemContainerForItemProperty, item);
            if (container == item)
                return;
            _map.Add(item, container);
            host.ItemTemplate.SetBindings(container, item);
            DependencyProperty dp = FrameworkElement.DataContextProperty;
            container.SetValue(dp, item);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                    this.Reset(_itemsControl3D.Items);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
