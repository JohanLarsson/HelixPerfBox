namespace HelixPerfBox
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media.Media3D;

    public class ItemsControl3D : ModelVisual3D
    {
        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(
            typeof(ItemsControl3D),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnItemsSourceChanged));

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate",
            typeof(TemplateModel),
            typeof(ItemsControl3D),
            new PropertyMetadata(default(TemplateModel)));

        private readonly ConcurrentQueue<Visual3D> _cache = new ConcurrentQueue<Visual3D>();

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public TemplateModel ItemTemplate
        {
            get { return (TemplateModel)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl3D = (ItemsControl3D)o;
            itemsControl3D.Children.Clear();
            var old = e.OldValue as INotifyCollectionChanged;
            if (old != null)
            {
                CollectionChangedEventManager.RemoveHandler(old, itemsControl3D.Handler);
            }

            var collectionChanged = e.NewValue as INotifyCollectionChanged;
            if (collectionChanged != null)
            {
                CollectionChangedEventManager.AddHandler(collectionChanged, itemsControl3D.Handler);
            }
            var items = e.NewValue as IEnumerable;
            if (items == null)
            {
                return;
            }
            itemsControl3D.Update(items);
        }

        private void Handler(object sender, NotifyCollectionChangedEventArgs e)
        {
            Update((IEnumerable)sender);
        }

        protected void Update(IEnumerable items)
        {
            foreach (var child in Children)
            {
                //BindingOperations.ClearAllBindings(child);
                _cache.Enqueue(child);
            }
            Children.Clear();
            foreach (var item in items)
            {
                Visual3D visual3D;
                if (!_cache.TryDequeue(out visual3D))
                {
                    visual3D = ItemTemplate.Create();
                }
                ItemTemplate.SetBindings(visual3D, item);
                Children.Add(visual3D);
            }
        }
    }
}