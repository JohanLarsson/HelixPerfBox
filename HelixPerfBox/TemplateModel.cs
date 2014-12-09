namespace HelixPerfBox
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media.Media3D;

    [DictionaryKeyProperty("DataTemplateKey")]
    [ContentProperty("Model")]
    [Localizability(LocalizationCategory.NeverLocalize)] // All properties on template are not localizable
    public class TemplateModel : Freezable, INameScope
    {
        private Type _dataType;
        private PropertyInfo[] _dataTypeProperties;
        private ModelVisual3D _model;
        private IEnumerable<DependencyProperty> _dependencyProperties;
        private IEnumerable<Binding> _bindings;
        private IEnumerable<BindingExpressionBase> _bindingExpressions;
        public TemplateModel()
        {
        }

        public TemplateModel(object dataType)
        {
            DataType = dataType;
        }

        [DefaultValue(null)]
        public object DataType
        {
            get { return _dataType; }
            set
            {
                CheckSealed();
                _dataType = (Type)value;
                _dataTypeProperties = _dataType.GetProperties();
                VerifyBindings();
            }
        }

        /// <summary>
        ///     The key that will be used if the DataTemplate is added to a
        ///     ResourceDictionary in Xaml without a specified Key (x:Key).
        /// </summary>
        public object DataTemplateKey
        {
            get
            {
                return (DataType != null) ? new DataTemplateKey(DataType) : null;
            }
        }

        /// <summary>
        ///     Root node of the template
        /// </summary>
        public ModelVisual3D Model
        {
            get
            {
                VerifyAccess();
                return _model;
            }
            set
            {
                VerifyAccess();
                CheckSealed();
                _model = value;
                _bindings = _model.Bindings();
                _bindingExpressions = _model.BindingExpressions();
                _dependencyProperties = _model.DependencyProperties();
                VerifyBindings();
            }
        }

        public IEnumerable<Binding> Bindings
        {
            get { return _bindings; }
        }

        public ModelVisual3D Create()
        {
            var clone = Model.Content.CloneCurrentValue();
            var visual3D = (ModelVisual3D)Activator.CreateInstance(Model.GetType());
            visual3D.Content = clone;
            foreach (var dependencyProperty in _dependencyProperties)
            {
                if (dependencyProperty == ModelVisual3D.ContentProperty)
                {
                    continue;
                }
                var value = Model.GetValue(dependencyProperty);
                visual3D.SetCurrentValue(dependencyProperty, value);
            }
            return visual3D;
        }

        public void SetBindings(Visual3D visual3D, object source)
        {
            visual3D.SetBindings(_bindingExpressions, source);
        }

        #region INameScope

        private readonly NameScope _nameScope = new NameScope();

        /// <summary>
        /// Registers the name - Context combination
        /// </summary>
        /// <param name="name">Name to register</param>
        /// <param name="scopedElement">Element where name is defined</param>
        public void RegisterName(string name, object scopedElement)
        {
            VerifyAccess();
            _nameScope.RegisterName(name, scopedElement);
        }

        /// <summary>
        /// Unregisters the name - element combination
        /// </summary>
        /// <param name="name">Name of the element</param>
        public void UnregisterName(string name)
        {
            VerifyAccess();
            _nameScope.UnregisterName(name);
        }

        /// <summary>
        /// Find the element given name
        /// </summary>
        /// <param name="name">Name of the element</param>
        object INameScope.FindName(string name)
        {
            VerifyAccess();
            return _nameScope.FindName(name);
        }

        #endregion INameScope

        internal void CheckSealed()
        {
            if (IsSealed)
            {
                throw new InvalidOperationException("Cannot be changed after it is sealed");
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new TemplateModel(_dataType);
        }

        private void VerifyBindings()
        {
            if (_bindingExpressions == null)
            {
                return;
            }
            foreach (var expression in _bindingExpressions)
            {
                var binding = expression.ParentBindingBase as Binding;
                if (binding == null)
                {
                    throw new NotImplementedException(string.Format("Not handling bindings of type {0}", expression.ParentBindingBase.GetType().Name));
                }
                if (_dataTypeProperties == null || binding.RelativeSource != null || binding.ElementName != null)
                {
                    continue;
                }
                if (_dataTypeProperties.All(x => x.Name != binding.Path.Path))
                {
                    throw new ArgumentException(string.Format("Trying to bind to {0}.{1}", _dataType.Name, binding.Path.Path));
                }
            }
        }

    }
}
