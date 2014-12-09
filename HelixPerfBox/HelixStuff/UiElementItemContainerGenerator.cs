namespace HelixPerfBox
{
    using System.Windows.Media.Media3D;

    public class UiElementItemContainerGenerator : ItemContainerGenerator3D
    {
        public UiElementItemContainerGenerator(ItemsControl3D parent)
            : base(parent)
        {
        }

        protected override Visual3D CreateNewContainer()
        {
            var modelVisual3D = Parent.ItemTemplate.Create();
            var container3D = new UIElementItemContainer3D(modelVisual3D);
        }
    }
}