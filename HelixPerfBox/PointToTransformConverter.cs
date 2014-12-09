namespace HelixPerfBox
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media.Media3D;

    public class PointToTransformConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Point3D))
            {
                return new TranslateTransform3D(0, 0, 0);
            }
            var point3D = (Point3D)value;
            return new TranslateTransform3D(point3D.X, point3D.Y, point3D.Z);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RadiusToTransformConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is double))
            {
                return new ScaleTransform3D(0, 0, 0);
            }
            var r = (double)value;
            return new ScaleTransform3D(new Vector3D(r, r, r));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PointAndRadiusToTransformConverter : IMultiValueConverter
    {
        private static readonly PointToTransformConverter PointToTransformConverter = new PointToTransformConverter();
        private static readonly RadiusToTransformConverter RadiusToTransformConverter = new RadiusToTransformConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var translate = (TranslateTransform3D)PointToTransformConverter.Convert(values[0], null, null, null);
            var scale = (ScaleTransform3D)RadiusToTransformConverter.Convert(values[1], null, null, null);

            var transform3DGroup = new Transform3DGroup();
            transform3DGroup.Children.Add(scale); // Order important here, scale then translate
            transform3DGroup.Children.Add(translate);
            return transform3DGroup;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
