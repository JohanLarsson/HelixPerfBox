// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspMvcViewLocationFormatAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The asp mvc view location format attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox.Annotations
{
    using System;

    /// <summary>
    /// The asp mvc view location format attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AspMvcViewLocationFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspMvcViewLocationFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        public AspMvcViewLocationFormatAttribute(string format) { }
    }
}