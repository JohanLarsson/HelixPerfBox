namespace HelixPerfBox
{
    using System.Windows;
    using System.Windows.Media;

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
            FillBox.Items.Add(Brushes.Black);
            FillBox.Items.Add(Brushes.Red);
            FillBox.Items.Add(Brushes.Blue);
            FillBox.Items.Add(Brushes.HotPink);
        }

        private void OnZoomExtentsClick(object sender, RoutedEventArgs e)
        {
            //Viewport3D.ZoomExtents(300);
        }
    }
}
