namespace HelixPerfBox
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;
    using System.Windows.Media.Media3D;
    using System.Windows.Threading;
    using HelixToolkit.Wpf;

    [ContentProperty("Children")]
    public class Selector3D : UIElement3D
    {
        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(
            typeof(Selector3D),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnItemsSourceChanged));

        public static readonly DependencyProperty SelectedItemProperty = Selector.SelectedItemProperty.AddOwner(
            typeof(Selector3D),
            new PropertyMetadata(null, OnSelectedItemChanged));

        private readonly Visual3DCollection _children;
        private readonly ConcurrentQueue<ItemContainer3D> _containers = new ConcurrentQueue<ItemContainer3D>();

        public Selector3D()
        {
            var ctor = typeof(Visual3DCollection).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
            _children = (Visual3DCollection)ctor.Invoke(new object[] { this });
            //Task.Run(() => CreateGeometries(this.Dispatcher));
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
            get { return _children; }
        }

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        protected override int Visual3DChildrenCount
        {
            get { return _children.Count; }
        }

        protected override Visual3D GetVisual3DChild(int index)
        {
            return _children[index];
        }

        private static async void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var selector3D = (Selector3D)o;
            selector3D.Children.Clear();
            var old = e.OldValue as INotifyCollectionChanged;
            if (old != null)
            {
                CollectionChangedEventManager.RemoveHandler(old, selector3D.Handler);
            }
            var items = e.NewValue as IEnumerable;
            if (items == null)
            {
                return;
            }
            selector3D.Update(items);
            var collectionChanged = items as INotifyCollectionChanged;
            if (collectionChanged != null)
            {
                CollectionChangedEventManager.AddHandler(collectionChanged, selector3D.Handler);
            }
        }

        private void Handler(object sender, NotifyCollectionChangedEventArgs e)
        {
            Update((IEnumerable)sender);
        }

        private void Update(IEnumerable items)
        {
            foreach (var child in Children)
            {
                _containers.Enqueue((ItemContainer3D)child);
            }
            Children.Clear();
            foreach (Ball item in items)
            {
                ItemContainer3D container;
                if (_containers.TryDequeue(out container))
                {
                    Children.Add(container);
                }
                else
                {
                    var builder = new MeshBuilder();
                    builder.AddSphere(new Point3D(0, 0, 0), 1);
                    var model3D = new GeometryModel3D { Geometry = builder.ToMesh(), Material = Materials.Orange };
                    var container3D = new ItemContainer3D(model3D);
                    var transform3DGroup = new Transform3DGroup();
                    transform3DGroup.Children.Add(new ScaleTransform3D(item.Radius, item.Radius, item.Radius));
                    transform3DGroup.Children.Add( new TranslateTransform3D(item.Point3D.X, item.Point3D.Y, item.Point3D.Z));
                    model3D.Transform = transform3DGroup;
                    Children.Add(container3D);
                }
            }
        }

        private static void OnSelectedItemChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            //var selector3D = (Selector3D) o;
            //UIElement3D element;
            //if (e.OldValue != null)
            //{
            //    if (selector3D._map.TryGetValue(e.OldValue, out element))
            //    {
            //        //element
            //    }
            //}
            //if (e.NewValue != null)
            //{

            //}
        }

        private void CreateGeometries(Dispatcher dispatcher)
        {
            var stopwatch = Stopwatch.StartNew();
            var n = 100;
            var model3Ds = new GeometryModel3D[n];
            Parallel.For(0, n, i =>
            {
                var builder = new MeshBuilder();
                builder.AddSphere(new Point3D(0, 0, 0), 1);
                var model3D = new GeometryModel3D { Geometry = builder.ToMesh(), Material = Materials.Orange };
                model3Ds[i] = model3D;
            });
            Debug.WriteLine("Created {0} geometries took: {1} ms", n, stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            dispatcher.Invoke(() =>
            {
                for (int i = 0; i < n; i++)
                {
                    var geometryModel3D = model3Ds[i];
                    _containers.Enqueue(new ItemContainer3D(geometryModel3D.Clone()));
                }
            });
            Debug.WriteLine("Created {0} containers took: {1} ms", n, stopwatch.ElapsedMilliseconds);
        }
    }
}
