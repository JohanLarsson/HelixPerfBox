// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspMvcAreaViewLocationFormatAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The asp mvc area view location format attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox.Annotations
{
    using System;

    /// <summary>
    /// The asp mvc area view location format attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AspMvcAreaViewLocationFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspMvcAreaViewLocationFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        public AspMvcAreaViewLocationFormatAttribute(string format) { }
    }
}