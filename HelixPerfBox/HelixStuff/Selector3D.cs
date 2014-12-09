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
    using System.Windows.Data;
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
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        private readonly Visual3DCollection _children;
        private readonly ConcurrentQueue<UIElementItemContainer3D> _containers = new ConcurrentQueue<UIElementItemContainer3D>();

        public Selector3D()
        {
            _children = Visual3DCollectionExt.Create(this);
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
                var itemContainer3D = (UIElementItemContainer3D)child;
                BindingOperations.ClearAllBindings(itemContainer3D);
                _containers.Enqueue(itemContainer3D);
            }
            Children.Clear();
            foreach (Ball item in items)
            {
                UIElementItemContainer3D container;
                if (_containers.TryDequeue(out container))
                {
                    AddItem(item, container);
                }
                else
                {
                    var builder = new MeshBuilder();
                    builder.AddSphere(new Point3D(0, 0, 0), 1);
                    var model3D = new GeometryModel3D { Geometry = builder.ToMesh(), Material = Materials.Orange };
                    var container3D = new UIElementItemContainer3D(model3D);

                    AddItem(item, container3D);
                }
            }
        }
        private void AddItem(Ball item, UIElementItemContainer3D container3D)
        {

            var model3D = container3D.Model;
            if (model3D != null)
            {
                var transform = new Transform3DGroup();
                transform.Children.Add(new ScaleTransform3D(item.Radius, item.Radius, item.Radius));
                transform.Children.Add(new TranslateTransform3D(item.Point3D.X, item.Point3D.Y, item.Point3D.Z));
                model3D.Transform = transform;
                container3D.Bind(UIElementItemContainer3D.ItemProperty, item);
            }
            Children.Add(container3D);
        }

        private static void OnSelectedItemChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var selector3D = (Selector3D)o;
            if (e.OldValue != null)
            {
                var container3D = selector3D.GetContainerForItem(e.OldValue);
                if (container3D != null)
                {
                    container3D.IsSelected = false;
                }
            }
            if (e.NewValue != null)
            {
                var container3D = selector3D.GetContainerForItem(e.NewValue);
                if (container3D != null)
                {
                    container3D.IsSelected = true;
                }
            }
        }

        internal UIElementItemContainer3D GetContainerForItem(object item)
        {
            return Children.OfType<UIElementItemContainer3D>().FirstOrDefault(x => x.Item == item);
        }
    }
}
