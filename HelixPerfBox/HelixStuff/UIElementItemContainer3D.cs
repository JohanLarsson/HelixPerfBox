namespace HelixPerfBox
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;


    // ReSharper disable once InconsistentNaming
    [ContentProperty("Child")]
    public class UIElementItemContainer3D : UIElement3D
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(UIElementItemContainer3D),
            new PropertyMetadata(default(bool), OnIsSelectedChanged));

        private static readonly DependencyProperty OriginalMaterialProperty = DependencyProperty.Register(
            "OriginalMaterial",
            typeof(Material),
            typeof(UIElementItemContainer3D),
            new PropertyMetadata(Materials.Brown));

        public static readonly DependencyProperty ItemProperty = ItemContainer3D.ItemProperty.AddOwner(
            typeof(UIElementItemContainer3D));

        private readonly WeakReference _parent = new WeakReference(null);
        private Visual3D _child;

        public UIElementItemContainer3D()
        {
        }

        public UIElementItemContainer3D(Visual3D child)
        {
            Child = child;
            //Visual3DModel = ((ModelVisual3D)child).Content;
        }

        [Obsolete("Visual3DModel = modelVisual3D.Content; can't be right")]
        public Visual3D Child
        {
            get { return _child; }
            set
            {
                if (Equals(_child, value))
                {
                    return;
                }
                if (_child != null)
                {
                    RemoveVisual3DChild(_child);
                }
                _child = value;
                if (_child != null)
                {
                    AddVisual3DChild(_child);
                    var modelVisual3D = _child as ModelVisual3D;
                    if (modelVisual3D != null)
                    {
                        Visual3DModel = modelVisual3D.Content;
                    }
                }

            }
        }

        public Selector3D Parent { get { return (Selector3D)_parent.Target; } }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public object Item
        {
            get { return (object)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var parent = (Selector3D)VisualTreeHelper.GetParent(this);
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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && Parent != null)
            {
                IsSelected = true;
            }
            e.Handled = true;
        }

        protected override int Visual3DChildrenCount
        {
            get
            {
                return Child == null ? 0 : 1;
            }
        }

        protected override Visual3D GetVisual3DChild(int index)
        {
            if (Child == null || index != 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return Child;
        }

        protected virtual void OnIsSelectedChanged(DependencyPropertyChangedEventArgs e)
        {
            var model3D = Visual3DModel as GeometryModel3D;
            if (model3D == null)
            {
                return;
            }
            if (IsSelected)
            {
                Parent.SelectedItem = Item;
                model3D.SetCurrentValue(OriginalMaterialProperty, model3D.Material);
                model3D.SetCurrentValue(GeometryModel3D.MaterialProperty, Materials.Orange);
            }
            else
            {
                if (Parent.SelectedItem == Item)
                {
                    Parent.SelectedItem = null;
                }
                var binding = BindingOperations.GetBindingExpressionBase(model3D, GeometryModel3D.MaterialProperty);
                if (binding != null)
                {
                    binding.UpdateTarget();
                }
                else
                {
                    var reset = model3D.GetValue(OriginalMaterialProperty);
                    model3D.SetCurrentValue(GeometryModel3D.MaterialProperty, reset);
                }
            }
        }

        private static void OnIsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var container3D = (UIElementItemContainer3D)o;
            container3D.OnIsSelectedChanged(e);
        }
    }
}