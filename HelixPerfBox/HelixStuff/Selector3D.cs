// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Selector3D.cs" company="">
//   
// </copyright>
// <summary>
//   The selector 3 d.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;

    /// <summary>
    /// The selector 3 d.
    /// </summary>
    [ContentProperty("Children")]
    public class Selector3D : ItemsControl3D
    {
        /// <summary>
        /// The selected item property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = Selector.SelectedItemProperty.AddOwner(
            typeof(Selector3D), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// The on selected item changed.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnSelectedItemChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var selector3D = (Selector3D)o;
            if (e.OldValue != null)
            {
                var container3D = selector3D.GetContainerForItem(e.OldValue) as UIElementItemContainer3D;
                if (container3D != null)
                {
                    container3D.IsSelected = false;
                }
            }

            if (e.NewValue != null)
            {
                var container3D = selector3D.GetContainerForItem(e.NewValue) as UIElementItemContainer3D;
                if (container3D != null)
                {
                    container3D.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// The create item container generator.
        /// </summary>
        /// <returns>
        /// The <see cref="ItemContainerGenerator3D"/>.
        /// </returns>
        protected override ItemContainerGenerator3D CreateItemContainerGenerator()
        {
            return new UiElementItemContainerGenerator(this);
        }
    }
}
