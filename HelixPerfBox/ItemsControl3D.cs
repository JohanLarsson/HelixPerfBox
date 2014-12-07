namespace HelixPerfBox
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    public abstract class ItemsControl3D<T> : ModelVisual3D
    {
        private readonly Action<MeshBuilder, T> _meshAction;

        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(
            typeof(ItemsControl3D<T>),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnItemsSourceChanged));

        public static readonly DependencyProperty MaterialProperty = DependencyProperty.Register(
            "Material",
            typeof(Material),
            typeof(ItemsControl3D<T>),
            new PropertyMetadata(default(Material)));

        protected ItemsControl3D(Action<MeshBuilder, T> meshAction)
        {
            _meshAction = meshAction;
        }

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public Material Material
        {
            get { return (Material)GetValue(MaterialProperty); }
            set { SetValue(MaterialProperty, value); }
        }

        private static async void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl3D = (ItemsControl3D<T>)o;
            itemsControl3D.Children.Clear();
            var old = e.OldValue as INotifyCollectionChanged;
            if (old != null)
            {
                CollectionChangedEventManager.RemoveHandler(old, itemsControl3D.Handler);
            }

            var collectionChanged = e.NewValue as INotifyCollectionChanged;
            if (collectionChanged != null)
            {
                CollectionChangedEventManager.AddHandler(collectionChanged, itemsControl3D.Handler);
            }
            var items = e.NewValue as IEnumerable<T>;
            if (items == null || !items.Any())
            {
                return;
            }
            itemsControl3D.Update(items);
        }

        private void Handler(object sender, NotifyCollectionChangedEventArgs e)
        {
            Update((IEnumerable<T>)sender);
        }

        protected async void Update(IEnumerable<T> items)
        {
            Children.Clear();
            var mesh = await Task.Run(() => CreateMesh(items, _meshAction, true));
            var model = new GeometryModel3D { Geometry = mesh, Material = Material };
            model.Bind(GeometryModel3D.MaterialProperty, this, MaterialProperty);
            Children.Add(new ModelVisual3D { Content = model });
        }

        private static MeshGeometry3D CreateMesh(IEnumerable<T> items, Action<MeshBuilder, T> meshAction, bool freeze = true)
        {
            var builder = new MeshBuilder();
            foreach (var item in items)
            {
                meshAction(builder, item);
            }
            var mesh = builder.ToMesh(freeze);
            return mesh;
        }
    }
}