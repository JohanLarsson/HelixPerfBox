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
        /// <returns>
        /// The <see cref="Visual3D"/>.
        /// </returns>
        protected override Visual3D CreateNewContainer()
        {
            var modelVisual3D = Parent.ItemTemplate.Create();
            var container3D = new UIElementItemContainer3D(modelVisual3D);
            return container3D;
        }
    }
}