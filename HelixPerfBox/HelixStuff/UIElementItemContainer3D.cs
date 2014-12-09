namespace HelixPerfBox
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    // ReSharper disable once InconsistentNaming
    public class UIElementItemContainer3D : UIElement3D
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(UIElementItemContainer3D),
            new PropertyMetadata(default(bool), OnIsSelectedChanged));

        public static readonly DependencyProperty ItemProperty = ItemContainer3D.ItemProperty.AddOwner(
            typeof(UIElementItemContainer3D));

        private Selector3D _parent;

        public UIElementItemContainer3D()
        {
        }

        public UIElementItemContainer3D(Model3D model)
        {
            Visual3DModel = model;
        }

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

        internal Model3D Model
        {
            get { return Visual3DModel; }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            _parent = (Selector3D)VisualTreeHelper.GetParent(this);
            if (_parent == null)
            {
                IsSelected = false;
            }
            else if (_parent.SelectedItem != null)
            {
                if (_parent.GetContainerForItem(_parent.SelectedItem) == this)
                {
                    IsSelected = true;
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed && _parent != null)
            {
                //_parent.SetCurrentValue(Selector3D.SelectedItemProperty, Item);
                _parent.SelectedItem = Item;
                IsSelected = true;
            }
        }

        private static void OnIsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var container3D = (UIElementItemContainer3D)o;
            var model3D = container3D.Visual3DModel as GeometryModel3D;
            if (model3D == null)
            {
                return;
            }
            model3D.Material = container3D.IsSelected
                ? Materials.Indigo
                : Materials.Orange;
        }
    }
}