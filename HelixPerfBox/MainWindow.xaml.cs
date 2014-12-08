namespace HelixPerfBox
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly SphereVisual3D[] _spheres = CreateSpheres(100).ToArray();
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new ViewModel();
            DataContext = viewModel;
            MaterialBox.Items.Add(Materials.Black);
            MaterialBox.Items.Add(Materials.Red);
            MaterialBox.Items.Add(Materials.Blue);
        }

        private void OnZoomExtentsClick(object sender, RoutedEventArgs e)
        {
            Viewport3D.ZoomExtents(300);
        }
    }
}
