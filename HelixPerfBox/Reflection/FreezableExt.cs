namespace HelixPerfBox
{
    using System.Reflection;
    using System.Windows;

    public static class FreezableExt
    {
        private static readonly MethodInfo AddInheritanceContextMethod = typeof(Freezable).GetMethod("AddInheritanceContext", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);       
        private static readonly MethodInfo RemoveInheritanceContextMethod = typeof(Freezable).GetMethod("RemoveInheritanceContext", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        public static readonly DependencyProperty DataContextProxyProperty = DependencyProperty.RegisterAttached(
            "DataContextProxy",
            typeof(object),
            typeof(FreezableExt),
            new PropertyMetadata(default(object), OnDataContextChanged));

        public static void SetDataContextProxy(this Freezable element, object value)
        {
            element.SetValue(DataContextProxyProperty, value);
        }

        public static object GetDataContextProxy(this Freezable element)
        {
            return (object)element.GetValue(DataContextProxyProperty);
        }

        /// <summary>
        /// Helper function to add context information to a Freezable.
        /// http://referencesource.microsoft.com/#WindowsBase/Base/System/Windows/Freezable.cs,1176
        /// </summary>
        /// <param name="context">The DependencyObject to add that references this Freezable.</param>
        /// <param name="property">The property of the DependencyObject this object maps to or null if none.</param>
        public static void AddInheritanceContextWithReflection(this Freezable self, DependencyObject context, DependencyProperty property)
        {
            AddInheritanceContextMethod.Invoke(self, new object[] { context, property });
        }

        /// <summary>
        /// Helper function to remove context information to a Freezable.
        /// http://referencesource.microsoft.com/#WindowsBase/Base/System/Windows/Freezable.cs,1204
        /// </summary>
        /// <param name="context">The DependencyObject to remove that references this Freezable.</param>
        /// <param name="property">The property of the DependencyObject this object maps to or null if none.</param>
        public static void RemoveInheritanceContextWithReflection(this Freezable self, DependencyObject context, DependencyProperty property)
        {
            RemoveInheritanceContextMethod.Invoke(self, new object[] { context, property });
        }

        private static void OnDataContextChanged(DependencyObject o, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var freezable = (Freezable)o;
            freezable.AddInheritanceContextWithReflection(o, DataContextProxyProperty);
        }
    }
}
