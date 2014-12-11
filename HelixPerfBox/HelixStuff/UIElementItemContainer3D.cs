// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIElementItemContainer3D.cs" company="">
//   
// </copyright>
// <summary>
//   The ui element item container 3 d.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    using HelixToolkit.Wpf;
    using Reflection;

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// The ui element item container 3 d.
    /// </summary>
    [ContentProperty("Child")]
    public class UIElementItemContainer3D : UIElement3D
    {
        /// <summary>
        /// The is selected property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(UIElementItemContainer3D),
            new PropertyMetadata(default(bool), OnIsSelectedChanged));

        public static readonly DependencyProperty DataContextProperty = FrameworkElement.DataContextProperty.AddOwner(
            typeof(UIElementItemContainer3D),
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.Inherits,
                OnDataContextChanged));

        /// <summary>
        /// The original material property.
        /// </summary>
        private static readonly DependencyProperty OriginalMaterialProperty = DependencyProperty.Register(
            "OriginalMaterial",
            typeof(Material),
            typeof(UIElementItemContainer3D),
            new PropertyMetadata(Materials.Brown));

        /// <summary>
        /// The item property.
        /// </summary>
        public static readonly DependencyProperty ItemProperty = ItemContainer3D.ItemProperty.AddOwner(
            typeof(UIElementItemContainer3D));

        private readonly WeakReference _parent = new WeakReference(null);
        private readonly DataContextProxy _dataContextProxy = new DataContextProxy();
        private readonly Visual3DCollection _children;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementItemContainer3D"/> class.
        /// </summary>
        public UIElementItemContainer3D()
        {
            _children = Visual3DCollectionExt.Create(this);
            _dataContextProxy.SetAsInheritanceContextFor(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementItemContainer3D"/> class.
        /// </summary>
        /// <param name="child">
        /// The child.
        /// </param>
        public UIElementItemContainer3D(Visual3D child)
            : this()
        {
            Child = child;
        }

        public object DataContext
        {
            get
            {
                return (object)GetValue(DataContextProperty);
            }
            set
            {
                SetValue(DataContextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the child.
        /// </summary>
        public Visual3D Child
        {
            get { return _children.FirstOrDefault(); }
            set
            {
                _children.Clear();
                _children.Add(value);
            }
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        public Selector3D Parent { get { return _parent.Target as Selector3D; } }

        /// <summary>
        /// Gets or sets a value indicating whether is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        public object Item
        {
            get { return (object)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        /// <summary>
        /// The on visual parent changed.
        /// </summary>
        /// <param name="oldParent">
        /// The old parent.
        /// </param>
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var parent = VisualTreeHelper.GetParent(this) as Selector3D;
            _parent.Target = parent;
            if (parent == null)
            {
                IsSelected = false;
            }
            else if (parent.SelectedItem != null)
            {
                if (parent.GetContainerForItem(parent.SelectedItem) == this)
                {
                    IsSelected = true;
                }
            }
        }

        /// <summary>
        /// The on mouse down.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsSelected = true;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Gets the visual 3 d children count.
        /// </summary>
        protected override int Visual3DChildrenCount
        {
            get { return _children.Count; }
        }

        /// <summary>
        /// The get visual 3 d child.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="Visual3D"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        protected override Visual3D GetVisual3DChild(int index)
        {
            return _children[index];
        }

        /// <summary>
        /// The on is selected changed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnIsSelectedChanged(DependencyPropertyChangedEventArgs e)
        {
            var modelVisual3D = Child as ModelVisual3D;
            if (modelVisual3D == null)
            {
                return;
            }
            var geometryModel3D = modelVisual3D.Content as GeometryModel3D;
            if (geometryModel3D == null)
            {
                return;
            }
            var parent = Parent;
            if (IsSelected)
            {
                if (parent != null)
                {
                    parent.SelectedItem = Item;
                }
                geometryModel3D.SetCurrentValue(OriginalMaterialProperty, geometryModel3D.Material);
                geometryModel3D.SetCurrentValue(GeometryModel3D.MaterialProperty, Materials.Orange);
            }
            else
            {
                if (parent != null && parent.SelectedItem == Item)
                {
                    parent.SelectedItem = null;
                }

                var binding = BindingOperations.GetBindingExpressionBase(geometryModel3D, GeometryModel3D.MaterialProperty);
                if (binding != null)
                {
                    binding.UpdateTarget();
                }
                else
                {
                    var reset = geometryModel3D.GetValue(OriginalMaterialProperty);
                    geometryModel3D.SetCurrentValue(GeometryModel3D.MaterialProperty, reset);
                }
            }
        }

        /// <summary>
        /// The on is selected changed.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnIsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var container3D = (UIElementItemContainer3D)o;
            container3D.OnIsSelectedChanged(e);
        }

        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemContainer3D = (UIElementItemContainer3D) d;
            itemContainer3D._dataContextProxy.DataContext = e.NewValue;
        }
    }
}