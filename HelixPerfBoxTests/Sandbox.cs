namespace HelixPerfBoxTests
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Data;

    using HelixPerfBox;
    using HelixPerfBox.Annotations;

    using NUnit.Framework;

    public class Sandbox
    {
        [Test, RequiresSTA]
        public void DataContext()
        {
            var freezableDummy = new FreezableDummy();
            var binding = new Binding("Name");
            var bindingExpressionBase = BindingOperations.SetBinding(freezableDummy, FreezableDummy.ValueProperty, binding);
            Assert.AreEqual(null, freezableDummy.Value);

            var vm = new DummyVm { Name = "Test" };
            freezableDummy.SetDataContextProxy(vm);
            Assert.AreEqual(vm, freezableDummy.GetDataContextProxy());
            bindingExpressionBase.UpdateTarget();
            Assert.AreEqual("Test", freezableDummy.Value);
        }
    }

    public class FreezableDummy : Freezable
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(FreezableDummy), new PropertyMetadata(default(string)));

        public string Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
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

    public class DummyVm : INotifyPropertyChanged
    {
        private string _name;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
