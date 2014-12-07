namespace HelixPerfBox
{
    using System.Windows.Media.Media3D;

    public class Ball
    {
        public Ball(Point3D point3D,  double radius)
        {
            Point3D = point3D;
            Radius = radius;
        }

        public Point3D Point3D { get; private set; }

        public double Radius { get; private set; }
    }
}