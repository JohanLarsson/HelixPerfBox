// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspMvcPartialViewLocationFormatAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The asp mvc partial view location format attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox.Annotations
{
    using System;

    /// <summary>
    /// The asp mvc partial view location format attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AspMvcPartialViewLocationFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspMvcPartialViewLocationFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        public AspMvcPartialViewLocationFormatAttribute(string format) { }
    }
}