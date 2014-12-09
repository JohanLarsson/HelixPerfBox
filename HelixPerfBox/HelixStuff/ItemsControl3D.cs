namespace HelixPerfBox
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
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

        internal static readonly DependencyPropertyKey HasItemsPropertyKey = DependencyProperty.RegisterReadOnly(
            "HasItems",
            typeof(bool),
            typeof(ItemsControl3D),
            new PropertyMetadata(false));

        public static readonly DependencyProperty HasItemsProperty = HasItemsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate",
            typeof(TemplateModel),
            typeof(ItemsControl3D),
            new PropertyMetadata(default(TemplateModel), OnItemTemplateChanged));

        private ItemCollection3D _items;
        private ItemContainerGenerator3D _itemContainerGenerator;

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public bool HasItems
        {
            get
            {
                return (bool)GetValue(HasItemsProperty);
            }
            protected set
            {
                SetValue(HasItemsPropertyKey, value);
            }
        }

        public ItemCollection3D Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ItemCollection3D(this);
                    _itemContainerGenerator = this.CreateItemContainerGenerator();
                    (_items).CollectionChanged += this.OnItemCollectionChanged;
                }
                return _items;
            }
        }

        public TemplateModel ItemTemplate
        {
            get { return (TemplateModel)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        protected internal Visual3D GetContainerForItem(object item)
        {
            return _itemContainerGenerator.GetContainerForItem(item);
        }

        private static void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl3D = (ItemsControl3D)o;
            var oldValue = (IEnumerable)e.OldValue;
            var newValue = (IEnumerable)e.NewValue;
            if (e.NewValue != null)
                itemsControl3D.Items.SetItemsSource(newValue);
            else
                itemsControl3D.Items.ClearItemsSource();
            itemsControl3D.OnItemsSourceChanged(oldValue, newValue);
        }

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemsControl3D)d).OnItemTemplateChanged((TemplateModel)e.OldValue, (TemplateModel)e.NewValue);
        }

        protected virtual void OnItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.SetValue(ItemsControl3D.HasItemsPropertyKey, this._items != null && !this._items.IsEmpty);
            this.OnItemsChanged(e);
        }

        protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
        }

        protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
        }

        protected virtual void OnItemTemplateChanged(TemplateModel oldItemTemplate, TemplateModel newItemTemplate)
        {
            if (_itemContainerGenerator == null)
                return;
            _itemContainerGenerator.Refresh();
        }

        /// <summary>
        /// Override this to inject custom containergenerator
        /// </summary>
        /// <returns></returns>
        protected virtual ItemContainerGenerator3D CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator3D(this);
        }
    }
}