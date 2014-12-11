namespace HelixPerfBox
{
    using System.Windows;
    using Reflection;

    /// <summary>
    /// The data context proxy.
    /// </summary>
    public class DataContextProxy : FrameworkElement
    {
        public void SetAsInheritanceContextFor(DependencyObject dependencyObject)
        {
            this.ProvideSelfAsInheritanceContextWithReflection(dependencyObject, null);
        }
    }
}