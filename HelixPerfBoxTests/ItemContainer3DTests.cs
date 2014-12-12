namespace HelixPerfBoxTests
{
    using System.Windows.Controls;
    using System.Windows.Data;

    using HelixPerfBox;
    using HelixPerfBox.Reflection;

    using HelixToolkit.Wpf;

    using NUnit.Framework;

    [RequiresSTA]
    public class UiElementItemContainer3DTests
    {
        private DummyVm _vm;

        private SphereVisual3D _sphere;

        private BindingExpressionBase _bindingExpression;

        private UIElementItemContainer3D _container;

        private Binding _binding;

        [SetUp]
        public void SetUp()
        {
            _vm = new DummyVm { Radius = 2 };
            _sphere = new SphereVisual3D { Radius = 1 };
            _binding = new Binding("Radius");
            _bindingExpression = BindingOperations.SetBinding(_sphere, SphereVisual3D.RadiusProperty, _binding);
            _container = new UIElementItemContainer3D
                             {
                                 Child = _sphere,
                                 DataContext = _vm
                             };
        }

        [Test, Explicit("Hangs")]
        public void InheritanceContext()
        {
            _container.ProvideSelfAsInheritanceContextWithReflection(_container, UIElementItemContainer3D.DataContextProperty);
            _bindingExpression.UpdateTarget();
            Assert.AreEqual(2, _sphere.Radius);
        }

        [Test]
        public void DataContextProxy()
        {
            Assert.Fail();
            //var dataContextProxy = new DataContextProxy { DataContext = _vm };
            //Assert.IsTrue(dataContextProxy.IsInitialized);
            //dataContextProxy.AddLogicalChild(_container);
            //Assert.AreEqual(2, _sphere.Radius);
        }

        [Test]
        public void ReferenceTextBlock()
        {
            var textBlock = new TextBlock();
            var bindingExpression = BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, _binding);
            bindingExpression.UpdateTarget();
            Assert.AreEqual("2", textBlock.Text);
        }
    }
}
