// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspMvcAreaPartialViewLocationFormatAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The asp mvc area partial view location format attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox.Annotations
{
    using System;

    /// <summary>
    /// The asp mvc area partial view location format attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AspMvcAreaPartialViewLocationFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspMvcAreaPartialViewLocationFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        public AspMvcAreaPartialViewLocationFormatAttribute(string format) { }
    }
}