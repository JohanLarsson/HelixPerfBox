namespace HelixPerfBox
{
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media.Media3D;

    [ContentProperty("Content")]
    public class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataContextProperty = FrameworkElement.DataContextProperty.AddOwner(typeof(BindingProxy), new PropertyMetadata(default(object)));

        public object DataContext
        {
            get
            {
                return this.GetDataContextProxy();
            }
            set
            {
                this.SetDataContextProxy(value);
            }
        }

        private Visual3D _content;

        public Visual3D Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
