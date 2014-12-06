namespace HelixPerfBox
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media.Media3D;
    using Gu.Reactive;
    using HelixToolkit.Wpf;

    public class Selector3D : ModelVisual3D
    {
        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(
            typeof(Selector3D),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsSourceChanged));

        private readonly ContainerUIElement3D _container;

        public Selector3D()
        {
            _container = new ContainerUIElement3D();
            Children.Add(_container);
        }

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static async void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var selector3D = (Selector3D)o;
            selector3D.Children.Clear();
            var balls = e.NewValue as IEnumerable<Ball>;
            if (balls == null)
            {
                return;
            }
            var stopwatch = Stopwatch.StartNew();
            var meshes = await Task.Run(() => balls.Select(b => Tuple.Create(b, CreateMesh(b))).ToArray());
            Debug.WriteLine("Create meshes: {0} ms", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            foreach (var mesh in meshes)
            {

                var element = new ModelUIElement3D();
                var ball = mesh.Item1;
                var geometry = new GeometryModel3D
                {
                    Geometry = mesh.Item2,
                    Material = new DiffuseMaterial(ball.Brush)
                };
                element.Model = geometry;
                Bind(geometry.Material, DiffuseMaterial.BrushProperty, ball, x => x.Brush);
                //Bind(sphere, SphereVisual3D.CenterProperty, ball, x => x.Point3D);
                //Bind(sphere, SphereVisual3D.RadiusProperty, ball, x => x.Radius);
                //element.Transform = new TranslateTransform3D(5, 0, 0);
                element.MouseDown += selector3D.ContainerElementMouseDown;
                selector3D._container.Children.Add(element);
            }
            Debug.WriteLine("Add points: {0} ms", stopwatch.ElapsedMilliseconds);

            //selector3D.AddRange(balls);
            //var list = Task.Run(() =>
            //{
            //    var visual3Ds = new List<Tuple<SphereVisual3D,Ball>>();
            //    foreach (var ball in balls)
            //    {
            //        var sphere = new SphereVisual3D
            //        {
            //            Material = new DiffuseMaterial(ball.Brush),
            //            Center = ball.Point3D,
            //            Radius = ball.Radius
            //        };
            //        //Bind(material, DiffuseMaterial.BrushProperty, ball, x => x.Brush);
            //        //Bind(sphere, SphereVisual3D.CenterProperty, ball, x => x.Point3D);
            //        //Bind(sphere, SphereVisual3D.RadiusProperty, ball, x => x.Radius);
            //        var tuple = Tuple.Create( sphere,ball);
            //        visual3Ds.Add(tuple);
            //    }
            //    return visual3Ds;
            //});
            //selector3D.Dispatcher.Invoke(() =>
            //{
            //    foreach (var visual3D in list)
            //    {
            //        selector3D.Children.Add(visual3D.Item1);
            //    }
            //});

        }
        private void ContainerElementMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void AddRange(IEnumerable<Ball> balls)
        {
            foreach (var ball in balls)
            {
                var sphere = new SphereVisual3D
                {
                    Material = new DiffuseMaterial(ball.Brush),
                    Center = ball.Point3D,
                    Radius = ball.Radius
                };
                Bind(sphere.Material, DiffuseMaterial.BrushProperty, ball, x => x.Brush);
                Bind(sphere, SphereVisual3D.CenterProperty, ball, x => x.Point3D);
                Bind(sphere, SphereVisual3D.RadiusProperty, ball, x => x.Radius);
                Children.Add(sphere);
            }
        }

        private static void Bind<TSource>(DependencyObject target,
                                          DependencyProperty property,
                                          TSource source,
                                          Expression<Func<TSource, object>> prop)
        {
            var binding = new Binding(NameOf.Property(prop))
            {
                Source = source,
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(target, property, binding);
        }

        private static MeshGeometry3D CreateMesh(Ball ball)
        {
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddSphere(ball.Point3D, ball.Radius, 100, 50);
            return meshBuilder.ToMesh(true);
        }
    }
}
