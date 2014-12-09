namespace HelixPerfBox
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media.Media3D;

    using HelixPerfBox.Annotations;

    public class Ball : INotifyPropertyChanged
    {
        private double _radius;

        private Point3D _point3D;

        public Ball(Point3D point3D,  double radius)
        {
            Point3D = point3D;
            Radius = radius;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Point3D Point3D
        {
            get
            {
                return _point3D;
            }
            set
            {
                if (value.Equals(_point3D))
                {
                    return;
                }
                _point3D = value;
                OnPropertyChanged();
            }
        }

        public double Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                if (value.Equals(_radius))
                {
                    return;
                }
                _radius = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return string.Format("Point3D: {0}, Radius: {1}", this.Point3D, this.Radius);
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