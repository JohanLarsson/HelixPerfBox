// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemContainer3D.cs" company="">
//   
// </copyright>
// <summary>
//   The item container 3 d.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System.Windows;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// The item container 3 d.
    /// </summary>
    public class ItemContainer3D : ModelVisual3D
    {
        /// <summary>
        /// The item property.
        /// </summary>
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            "Item", 
            typeof(object), 
            typeof(ItemContainer3D), 
            new PropertyMetadata(default(object)));

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        public object Item
        {
            get { return (object)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
    }
}