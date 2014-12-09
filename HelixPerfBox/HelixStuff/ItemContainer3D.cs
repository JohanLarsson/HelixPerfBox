namespace HelixPerfBox
{
    using System.Windows;
    using System.Windows.Media.Media3D;

    public class ItemContainer3D : ModelVisual3D
    {
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            "Item",
            typeof(object),
            typeof(ItemContainer3D),
            new PropertyMetadata(default(object)));

        public object Item
        {
            get { return (object)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
    }
}