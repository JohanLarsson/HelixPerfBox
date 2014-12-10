// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointToTransformConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The point to transform converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// The point to transform converter.
    /// </summary>
    public class PointToTransformConverter : IValueConverter
    {
        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Point3D))
            {
                return new TranslateTransform3D(0, 0, 0);
            }

            var point3D = (Point3D)value;
            return new TranslateTransform3D(point3D.X, point3D.Y, point3D.Z);
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The radius to transform converter.
    /// </summary>
    public class RadiusToTransformConverter : IValueConverter
    {
        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is double))
            {
                return new ScaleTransform3D(0, 0, 0);
            }

            var r = (double)value;
            return new ScaleTransform3D(new Vector3D(r, r, r));
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The point and radius to transform converter.
    /// </summary>
    public class PointAndRadiusToTransformConverter : IMultiValueConverter
    {
        /// <summary>
        /// The point to transform converter.
        /// </summary>
        private static readonly PointToTransformConverter PointToTransformConverter = new PointToTransformConverter();

        /// <summary>
        /// The radius to transform converter.
        /// </summary>
        private static readonly RadiusToTransformConverter RadiusToTransformConverter = new RadiusToTransformConverter();

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var translate = (TranslateTransform3D)PointToTransformConverter.Convert(values[0], null, null, null);
            var scale = (ScaleTransform3D)RadiusToTransformConverter.Convert(values[1], null, null, null);

            var transform3DGroup = new Transform3DGroup();
            transform3DGroup.Children.Add(scale); // Order important here, scale then translate
            transform3DGroup.Children.Add(translate);
            return transform3DGroup;
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetTypes">
        /// The target types.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object[]"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
