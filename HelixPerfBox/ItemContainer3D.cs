namespace HelixPerfBox
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    public class ItemContainer3D : UIElement3D
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(ItemContainer3D),
            new PropertyMetadata(default(bool)));

        public ItemContainer3D()
        {
        }

        public ItemContainer3D(GeometryModel3D model)
        {
            Visual3DModel = model;
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var gm = Visual3DModel as GeometryModel3D;
                gm.Material = gm.Material == Materials.Blue ? Materials.Red : Materials.Blue;
                e.Handled = true;
            }
        }
    }
}