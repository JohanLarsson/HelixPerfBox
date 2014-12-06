namespace HelixPerfBox
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media.Media3D;
    using Gu.Reactive;
    using HelixToolkit.Wpf;

    using MS.Internal;

    [ContentProperty("Children")]
    public class Selector3D : UIElement3D
    {
        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(
            typeof(Selector3D),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsSourceChanged));

        private readonly Visual3DCollection _children;

        public Selector3D()
        {
            _children = (Visual3DCollection)Activator.CreateInstance(typeof(Visual3DCollection), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, new object[] { this });
        }

        /// <summary>
        /// Gets a <see cref="T:System.Windows.Media.Media3D.Visual3DCollection"/> of child elements of this <see cref="T:System.Windows.Media.Media3D.ContainerUIElement3D"/> object.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.Windows.Media.Media3D.Visual3DCollection"/> of child elements. The default is an empty collection.
        /// </returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Visual3DCollection Children
        {
            get
            {
                return _children;
            }
        }
        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        protected override int Visual3DChildrenCount
        {
            get
            {
                return _children.Count;
            }
        }

        protected override Visual3D GetVisual3DChild(int index)
        {
            return _children[index];
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
            foreach (var tuple in meshes)
            {
                var ball = tuple.Item1;
                var geometry = tuple.Item2;
                var material = new DiffuseMaterial(ball.Brush);
                Bind(material, DiffuseMaterial.BrushProperty, ball, x => x.Brush);

                var model = new GeometryModel3D { Geometry = geometry, Material = material };
                var element = new ModelUIElement3D { Model = model };
                element.AddHandler(UIElement3D.MouseDownEvent, new RoutedEventHandler(selector3D.ContainerElementMouseDown), true);
                element.MouseLeftButtonDown += selector3D.ContainerElementMouseDown;
                //var parent =(Viewport3DVisual) VisualTreeHelper.GetParent(selector3D);
                selector3D.Children.Add(element);
            }
            Debug.WriteLine("Add points: {0} ms", stopwatch.ElapsedMilliseconds);
        }

        private void ContainerElementMouseDown(object sender, RoutedEventArgs e)
        {
            var element = (ModelUIElement3D)sender;
            var gm = (GeometryModel3D)element.Model;
            gm.Material = Equals(gm.Material, Materials.Blue) ? Materials.Red : Materials.Blue;
            e.Handled = true;
        }

        //private void AddRange(IEnumerable<Ball> balls)
        //{
        //    foreach (var ball in balls)
        //    {
        //        var sphere = new SphereVisual3D
        //        {
        //            Material = new DiffuseMaterial(ball.Brush),
        //            Center = ball.Point3D,
        //            Radius = ball.Radius
        //        };
        //        Bind(sphere.Material, DiffuseMaterial.BrushProperty, ball, x => x.Brush);
        //        Bind(sphere, SphereVisual3D.CenterProperty, ball, x => x.Point3D);
        //        Bind(sphere, SphereVisual3D.RadiusProperty, ball, x => x.Radius);
        //        Children.Add(sphere);
        //    }
        //}

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
            var meshBuilder = new MeshBuilder(false, true);
            meshBuilder.AddSphere(ball.Point3D, ball.Radius, 100, 50);
            var geometry3D = meshBuilder.ToMesh(true);
            geometry3D.Freeze();
            return geometry3D;
        }
    }
}
