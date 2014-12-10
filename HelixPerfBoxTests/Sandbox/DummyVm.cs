namespace HelixPerfBoxTests
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using HelixPerfBox.Annotations;

    public class DummyVm : INotifyPropertyChanged
    {
        private double _radius;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                if (value == _radius)
                {
                    return;
                }
                _radius = value;
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