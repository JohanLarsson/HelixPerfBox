namespace HelixPerfBox
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Specialized;
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
            Remove(_itemsControl3D.Children);
        }

        public void Remove(IEnumerable oldItems)
        {
            foreach (var remove in oldItems)
            {
                Visual3D container;
                if (!_map.TryGetValue(remove, out container))
                {
                    throw new InvalidOperationException("Could not find container for item"); 
                }
                _itemsControl3D.Children.Remove(container);
                UnlinkContainerFromItem(container, _itemsControl3D);
                Recycle(container);
            }
        }

        public void Add(IEnumerable newItems)
        {
            foreach (var newItem in newItems)
            {
                var container = GenerateNext();
                LinkContainerForItem(container, newItem, _itemsControl3D);
                _itemsControl3D.Children.Add(container);
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
                    Remove(e.OldItems);
                    Add(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    return;
                case NotifyCollectionChangedAction.Reset:
                    Refresh();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
