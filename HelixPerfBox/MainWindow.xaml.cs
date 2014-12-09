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

        private async void OnMoveCenter(object sender, RoutedEventArgs e)
        {
            //var toggleButton = (ToggleButton)sender;
            //int sign = 1;
            //while (toggleButton.IsChecked == true)
            //{
            //    if (SphereVisual3D.Center.X > 10)
            //    {
            //        sign = -1;
            //    }

            //    if (SphereVisual3D.Center.X < -10)
            //    {
            //        sign = 1;
            //    }
            //    SphereVisual3D.Center += new Vector3D(sign, 0, 0);
            //    await System.Windows.Threading.Dispatcher.Yield();
            //}
        }
    }
}
