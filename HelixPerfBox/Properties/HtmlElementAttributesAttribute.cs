// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlElementAttributesAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The html element attributes attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox.Annotations
{
    using System;

    /// <summary>
    /// The html element attributes attribute.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Field, Inherited = true)]
    public sealed class HtmlElementAttributesAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlElementAttributesAttribute"/> class.
        /// </summary>
        public HtmlElementAttributesAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlElementAttributesAttribute"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public HtmlElementAttributesAttribute([NotNull] string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        [NotNull] public string Name { get; private set; }
    }
}