namespace HelixPerfBox
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    public class KnownTypes
    {
    }

    public class KnownSphere : KnownType
    {
        private const string HackTransform = "HackTransform";
        public KnownSphere()
            : base(typeof(SphereVisual3D), SphereVisual3D.RadiusProperty, SphereVisual3D.CenterProperty)
        {
        }
        public override ModelVisual3D Create()
        {
            return new SphereVisual3D() { Center = new Point3D(0, 0, 0), Radius = 1 };
        }

        public override bool CanSetBinding(ModelVisual3D visual3D)
        {
            var transform3D = visual3D.Transform;
            if (transform3D == null)
            {
                return true;
            }
            return transform3D.GetName() == HackTransform;
        }

        public override void SetBindings(ModelVisual3D visual3D, Tuple<DependencyProperty, object>[] propertiesAndValues)
        {
            var transform3DGroup = new Transform3DGroup();
            transform3DGroup.SetName(HackTransform);
            var radius = propertiesAndValues.FirstOrDefault(x => x.Item1 == SphereVisual3D.RadiusProperty);
            if(radius == null)
            {
                
            }
        }
    }

    public abstract class KnownType
    {
        protected KnownType(Type type, params DependencyProperty[] knownProperties)
        {
            Type = type;
            KnownProperties = knownProperties;
        }

        public Type Type { get; private set; }

        public DependencyProperty[] KnownProperties { get; private set; }

        public abstract ModelVisual3D Create();

        public abstract bool CanSetBinding(ModelVisual3D visual3D);

        public abstract void SetBindings(ModelVisual3D visual3D, Tuple<DependencyProperty, object>[] propertiesAndValues);
    }
}
