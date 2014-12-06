namespace HelixPerfBox
{
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class Ball
    {
        public Ball(Point3D point3D, Brush brush, double radius)
        {
            Point3D = point3D;
            Brush = brush;
            Radius = radius;
        }

        public Point3D Point3D { get; private set; }

        public Brush Brush { get; private set; }

        public double Radius { get; private set; }
    }
}