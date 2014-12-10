// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspMvcMasterLocationFormatAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The asp mvc master location format attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox.Annotations
{
    using System;

    /// <summary>
    /// The asp mvc master location format attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AspMvcMasterLocationFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspMvcMasterLocationFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        public AspMvcMasterLocationFormatAttribute(string format) { }
    }
}