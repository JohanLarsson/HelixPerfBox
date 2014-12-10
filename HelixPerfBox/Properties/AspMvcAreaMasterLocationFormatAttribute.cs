// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspMvcAreaMasterLocationFormatAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The asp mvc area master location format attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox.Annotations
{
    using System;

    /// <summary>
    /// The asp mvc area master location format attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AspMvcAreaMasterLocationFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspMvcAreaMasterLocationFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        public AspMvcAreaMasterLocationFormatAttribute(string format) { }
    }
}