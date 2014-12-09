namespace HelixPerfBox
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media.Media3D;

    using HelixToolkit.Wpf;

    [ContentProperty("Children")]
    public class Selector3D : ItemsControl3D
    {
        public static readonly DependencyProperty SelectedItemProperty = Selector.SelectedItemProperty.AddOwner(
            typeof(Selector3D),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        public object SelectedItem
        {
            get { return (object)this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        //private void Update(IEnumerable items)
        //{
        //    foreach (var child in Children)
        //    {
        //        var itemContainer3D = (UIElementItemContainer3D)child;
        //        BindingOperations.ClearAllBindings(itemContainer3D);
        //        _containers.Enqueue(itemContainer3D);
        //    }
        //    Children.Clear();
        //    foreach (Ball item in items)
        //    {
        //        UIElementItemContainer3D container;
        //        if (_containers.TryDequeue(out container))
        //        {
        //            AddItem(item, container);
        //        }
        //        else
        //        {
        //            var builder = new MeshBuilder();
        //            builder.AddSphere(new Point3D(0, 0, 0), 1);
        //            var model3D = new GeometryModel3D { Geometry = builder.ToMesh(), Material = Materials.Orange };
        //            var container3D = new UIElementItemContainer3D(model3D);

        //            AddItem(item, container3D);
        //        }
        //    }
        //}
        //private void AddItem(Ball item, UIElementItemContainer3D container3D)
        //{

        //    var model3D = container3D.Model;
        //    if (model3D != null)
        //    {
        //        var transform = new Transform3DGroup();
        //        transform.Children.Add(new ScaleTransform3D(item.Radius, item.Radius, item.Radius));
        //        transform.Children.Add(new TranslateTransform3D(item.Point3D.X, item.Point3D.Y, item.Point3D.Z));
        //        model3D.Transform = transform;
        //        container3D.Bind(UIElementItemContainer3D.ItemProperty, item);
        //    }
        //    Children.Add(container3D);
        //}

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

        protected override ItemContainerGenerator3D CreateItemContainerGenerator()
        {
            return new UiElementItemContainerGenerator(this);
        }
    }
}
