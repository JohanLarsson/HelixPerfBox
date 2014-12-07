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

        //private void OnAdd100Click(object sender, RoutedEventArgs e)
        //{
            
        //    var material = new DiffuseMaterial(Brushes.Green);

        //    var stopwatch = Stopwatch.StartNew();
        //    foreach (var sphere in _spheres)
        //    {
        //        Viewport3D.Children.Add(sphere);
        //    }
        //    //for (int i = 0; i < _spheres.Length; i++)
        //    //{
        //    //    var sphere = _spheres[i];
        //    //    sphere.Center = new Point3D(0,0,i);
        //    //    sphere.Material = material;
        //    //    sphere.Radius = 0.3;
        //    //    Viewport3D.Children.Add(sphere);
        //    //}
        //    //for (int i = 0; i < 100; i++)
        //    //{
        //    //    var sphere = new SphereVisual3D
        //    //    {
        //    //        Center = new Point3D(0, 0, i),
        //    //        Radius = 0.3,
        //    //        Material = material
        //    //    };
        //    //    Viewport3D.Children.Add(sphere);
        //    //}
        //    Button.Content = string.Format("Add: ({0}) ms", stopwatch.ElapsedMilliseconds);

        //    Viewport3D.ZoomExtents(0);
        //}

        //private void OnVisibility(object sender, RoutedEventArgs e)
        //{
        //    var stopwatch = Stopwatch.StartNew();
        //    var toggleButton = ((ToggleButton)sender);
        //    var isChecked = toggleButton.IsChecked == true;
        //    if (isChecked)
        //    {
        //        foreach (var sphere in _spheres)
        //        {
        //            Viewport3D.Children.Add(sphere);
        //        }
        //    }
        //    else
        //    {
        //        Viewport3D.Children.Clear();
        //    }
        //    toggleButton.Content = string.Format("Visibility: ({0}) ms", stopwatch.ElapsedMilliseconds);
        //    Viewport3D.ZoomExtents(0);
        //}

        //private static IEnumerable<SphereVisual3D> CreateSpheres(int n)
        //{
        //    var material = new DiffuseMaterial(Brushes.Green);

        //    for (int i = 0; i < n; i++)
        //    {
        //        var sphere = new SphereVisual3D
        //        {
        //            Center = new Point3D(0, 0, i),
        //            Radius = 0.3,
        //            Material = material
        //        };
        //        yield return sphere;
        //    }
        //}
        private void OnZoomExtentsClick(object sender, RoutedEventArgs e)
        {
            Viewport3D.ZoomExtents(0);
        }
    }
}
