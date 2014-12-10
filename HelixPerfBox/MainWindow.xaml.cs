// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private readonly SphereVisual3D[] _spheres = CreateSpheres(100).ToArray();
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new ViewModel();
            DataContext = viewModel;
            FillBox.Items.Add(Brushes.Black);
            FillBox.Items.Add(Brushes.Red);
            FillBox.Items.Add(Brushes.Blue);
            FillBox.Items.Add(Brushes.HotPink);
        }

        /// <summary>
        /// The on zoom extents click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnZoomExtentsClick(object sender, RoutedEventArgs e)
        {
            // Viewport3D.ZoomExtents(300);
        }
    }
}
