using System;
using System.Windows;

namespace HelixPerfBox
{
    using System.Diagnostics;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MeshBuilder _meshBuilder = new MeshBuilder();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnAdd100Click(object sender, RoutedEventArgs e)
        {
            var material = new DiffuseMaterial(Brushes.Green);

            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var sphere = new SphereVisual3D
                {
                    Center = new Point3D(0, 0, i),
                    Radius = 0.3,
                    Material = material
                };
                Viewport3D.Children.Add(sphere);
            }
            Button.Content = stopwatch.ElapsedMilliseconds;
            Viewport3D.ZoomExtents(100);
        }
    }
}
