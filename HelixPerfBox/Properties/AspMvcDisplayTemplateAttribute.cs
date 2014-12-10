// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspMvcDisplayTemplateAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   ASP.NET MVC attribute. Indicates that a parameter is an MVC display template.
//   Use this attribute for custom wrappers similar to
//   <c>System.Web.Mvc.Html.DisplayExtensions.DisplayForModel(HtmlHelper, String)</c>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox.Annotations
{
    using System;

    /// <summary>
    /// ASP.NET MVC attribute. Indicates that a parameter is an MVC display template.
    /// Use this attribute for custom wrappers similar to 
    /// <c>System.Web.Mvc.Html.DisplayExtensions.DisplayForModel(HtmlHelper, String)</c>
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcDisplayTemplateAttribute : Attribute { }
}