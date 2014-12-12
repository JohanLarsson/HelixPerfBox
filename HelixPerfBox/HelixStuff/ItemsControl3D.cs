// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsControl3D.cs" company="">
//   
// </copyright>
// <summary>
//   The items control 3 d.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// The items control 3 d.
    /// </summary>
    [ContentProperty("Items")]
    [DefaultEvent("OnItemsChanged")]
    //[DefaultProperty("Items")]
    public class ItemsControl3D : ModelVisual3D
    {
        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(
            typeof(ItemsControl3D),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnItemsSourceChanged));

        /// <summary>
        /// The has items property key.
        /// </summary>
        internal static readonly DependencyPropertyKey HasItemsPropertyKey = DependencyProperty.RegisterReadOnly(
            "HasItems",
            typeof(bool),
            typeof(ItemsControl3D),
            new PropertyMetadata(false));

        /// <summary>
        /// The has items property.
        /// </summary>
        public static readonly DependencyProperty HasItemsProperty = HasItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// The item template property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate",
            typeof(TemplateModel),
            typeof(ItemsControl3D),
            new PropertyMetadata(default(TemplateModel), OnItemTemplateChanged));

        /// <summary>
        /// The _items.
        /// </summary>
        private ItemCollection3D _items;

        /// <summary>
        /// The _item container generator.
        /// </summary>
        private ItemContainerGenerator3D _itemContainerGenerator;

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether has items.
        /// </summary>
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

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ItemCollection3D Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ItemCollection3D(this);
                    _itemContainerGenerator = CreateItemContainerGenerator();
                    _items.CollectionChanged += OnItemCollectionChanged;
                }

                return _items;
            }
        }

        /// <summary>
        /// Gets or sets the item template.
        /// </summary>
        public TemplateModel ItemTemplate
        {
            get { return (TemplateModel)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
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
        protected internal Visual3D GetContainerForItem(object item)
        {
            return _itemContainerGenerator.GetContainerOrDefaultForItem(item);
        }

        /// <summary>
        /// The on items source changed.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
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

        /// <summary>
        /// The on item template changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemsControl3D)d).OnItemTemplateChanged((TemplateModel)e.OldValue, (TemplateModel)e.NewValue);
        }

        /// <summary>
        /// The on item collection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetValue(HasItemsPropertyKey, _items != null && !_items.IsEmpty);
            OnItemsChanged(e);
        }

        /// <summary>
        /// The on items changed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// The on items source changed.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
        }

        /// <summary>
        /// The on item template changed.
        /// </summary>
        /// <param name="oldItemTemplate">
        /// The old item template.
        /// </param>
        /// <param name="newItemTemplate">
        /// The new item template.
        /// </param>
        protected virtual void OnItemTemplateChanged(TemplateModel oldItemTemplate, TemplateModel newItemTemplate)
        {
            if (_itemContainerGenerator == null)
                return;
            _itemContainerGenerator.OnItemTemplateChanged(oldItemTemplate, newItemTemplate);
        }

        /// <summary>
        /// Override this to inject custom containergenerator
        /// </summary>
        /// <returns>
        /// The <see cref="ItemContainerGenerator3D"/>.
        /// </returns>
        protected virtual ItemContainerGenerator3D CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator3D(this);
        }
    }
}