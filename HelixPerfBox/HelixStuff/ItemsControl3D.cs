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

    using HelixPerfBox.Reflection;

    using MS.Internal.Controls;

    public class ItemsControl3D : ModelVisual3D, IContainItemStorage
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

        private ItemCollection _items;
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

        public ItemCollection Items
        {
            get
            {
                if (_items == null)
                {
                    _items = ItemCollectionExt.Create(this);
                    _itemContainerGenerator = new ItemContainerGenerator3D(this);
                    ((INotifyCollectionChanged)_items).CollectionChanged += this.OnItemCollectionChanged;
                }
                return _items;
            }
        }

        public TemplateModel ItemTemplate
        {
            get { return (TemplateModel)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl3D = (ItemsControl3D)o;
            var oldValue = (IEnumerable)e.OldValue;
            var newValue = (IEnumerable)e.NewValue;
            var beb = BindingOperations.GetBindingExpressionBase(o, ItemsControl.ItemsSourceProperty);
            if (beb != null)
                itemsControl3D.Items.SetItemsSourceWithReflection(newValue, (Func<object, object>)(x => beb.GetSourceItemWithReflection(x)));
            else if (e.NewValue != null)
                itemsControl3D.Items.SetItemsSourceWithReflection(newValue, (Func<object, object>)null);
            else
                itemsControl3D.Items.ClearItemsSourceWithReflection();
            itemsControl3D.OnItemsSourceChanged(oldValue, newValue);
        }

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemsControl3D)d).OnItemTemplateChanged((TemplateModel)e.OldValue, (TemplateModel)e.NewValue);
        }

        protected virtual void OnItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.SetValue(ItemsControl3D.HasItemsPropertyKey, this._items != null && !this._items.IsEmpty);
            if (e.Action == NotifyCollectionChangedAction.Reset)
                ((IContainItemStorage)this).Clear();
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

        #region ItemValueStorage

        object IContainItemStorage.ReadItemValue(object item, DependencyProperty dp)
        {
            return Helper.ReadItemValue(this, item, dp.GlobalIndex);
        }


        void IContainItemStorage.StoreItemValue(object item, DependencyProperty dp, object value)
        {
            Helper.StoreItemValue(this, item, dp.GlobalIndex, value);
        }

        void IContainItemStorage.ClearItemValue(object item, DependencyProperty dp)
        {
            Helper.ClearItemValue(this, item, dp.GlobalIndex);
        }

        void IContainItemStorage.ClearValue(DependencyProperty dp)
        {
            Helper.ClearItemValueStorage(this, new int[] { dp.GlobalIndex });
        }

        void IContainItemStorage.Clear()
        {
            Helper.ClearItemValueStorage(this);
        }

        #endregion
    }
}