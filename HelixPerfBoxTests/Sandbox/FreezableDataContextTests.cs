namespace HelixPerfBoxTests
{
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media.Media3D;

    using HelixPerfBox;

    using HelixToolkit.Wpf;

    using NUnit.Framework;
    [RequiresSTA]
    public class FreezableDataContextTests
    {
        private DummyVm _vm;

        [SetUp]
        public void SetUp()
        {
            _vm = new DummyVm { Radius = 1 };
        }

        [Test]
        public void DataContextFreezable()
        {
            var freezableDummy = new FreezableDummy();
            var binding = new Binding("Radius");
            var bindingExpressionBase = BindingOperations.SetBinding(freezableDummy, FreezableDummy.ValueProperty, binding);
            Assert.AreEqual(0, freezableDummy.Value);

            freezableDummy.SetDataContextProxy(_vm);
            Assert.AreEqual(_vm, freezableDummy.GetDataContextProxy());
            bindingExpressionBase.UpdateTarget();
            Assert.AreEqual(1, freezableDummy.Value);
        }

        [Test]
        public void DataContextSphereVisual3D()
        {
            var sphere = new SphereVisual3D();
            var binding = new Binding("Radius");
            var bindingExpressionBase = BindingOperations.SetBinding(sphere, SphereVisual3D.RadiusProperty, binding);
            Assert.AreEqual(1, sphere.Radius);
            var proxy = new BindingProxy { Content = sphere };
            proxy.SetDataContextProxy(_vm);
            _vm.Radius = 2;
            bindingExpressionBase.UpdateTarget();
            Assert.AreEqual(2, sphere.Radius);
        }
    }

    public class FreezableDummy : Freezable
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(FreezableDummy), new PropertyMetadata(0.0));

        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new FreezableDummy();
        }
    }
}
