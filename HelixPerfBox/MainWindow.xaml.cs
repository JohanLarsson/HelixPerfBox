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
    using System.Windows.Media.Media3D;

    using HelixToolkit.Wpf;

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

            //var sphereVisual3D = new SphereVisual3D { Radius = 0.5, Fill = Brushes.Yellow, Center = new Point3D(1, 1, 0) };
            //var container = new ContainerUIElement3D();
            //container.Children.Add(sphereVisual3D);
            //SandboxViewPort.Children.Add(container);
            //var visual3D = new SphereVisual3D { Radius = 0.5, Fill = Brushes.RosyBrown, Center = new Point3D(-1, 1, 0) };
            //var container3D = new UIElementItemContainer3D(visual3D);
            //SandboxViewPort.Children.Add(container3D);
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
            SandboxViewPort.ZoomExtents(300);
            
            // Viewport3D.ZoomExtents(300);
        }
    }
}
