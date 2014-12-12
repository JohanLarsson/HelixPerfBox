// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UiElementItemContainerGenerator.cs" company="">
//   
// </copyright>
// <summary>
//   The ui element item container generator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System.Collections.Specialized;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// The ui element item container generator.
    /// </summary>
    public class UiElementItemContainerGenerator : ItemContainerGenerator3D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UiElementItemContainerGenerator"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        public UiElementItemContainerGenerator(Selector3D parent)
            : base(parent)
        {
        }

        /// <summary>
        /// The create new container.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>
        /// The <see cref="Visual3D"/>.
        /// </returns>
        protected override Visual3D CreateNewContainer(object item)
        {
            var itemContainer3D = item as UIElementItemContainer3D;
            if (itemContainer3D != null)
            {
                return itemContainer3D;
            }
            var modelVisual3D = base.CreateNewContainer(item);
            var container3D = new UIElementItemContainer3D(modelVisual3D);
            //container3D.DataContext = item;
            return container3D;
        }

        protected override void ApplyTemplate(Visual3D container, object item)
        {
            var uiElementItemContainer3D = container as UIElementItemContainer3D;
            if (uiElementItemContainer3D != null)
            {
                var visual3D = uiElementItemContainer3D.Child;
                if (visual3D != null)
                {
                    Parent.ItemTemplate.Rebind(visual3D, item);
                }
            }
        }
    }
}