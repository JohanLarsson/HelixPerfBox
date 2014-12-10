// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ball.cs" company="">
//   
// </copyright>
// <summary>
//   The ball.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media.Media3D;

    using HelixPerfBox.Annotations;

    /// <summary>
    /// The ball.
    /// </summary>
    public class Ball : INotifyPropertyChanged
    {
        /// <summary>
        /// The _radius.
        /// </summary>
        private double _radius;

        /// <summary>
        /// The _point 3 d.
        /// </summary>
        private Point3D _point3D;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ball"/> class.
        /// </summary>
        /// <param name="point3D">
        /// The point 3 d.
        /// </param>
        /// <param name="radius">
        /// The radius.
        /// </param>
        public Ball(Point3D point3D,  double radius)
        {
            Point3D = point3D;
            Radius = radius;
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the point 3 d.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
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

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Point3D: {0}, Radius: {1}", Point3D, Radius);
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
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