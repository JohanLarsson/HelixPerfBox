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
            var proxy = new DataContextProxy { DataContext = value };
            element.SetValue(DataContextProxyProperty, proxy);
        }

        public static object GetDataContextProxy(this Freezable element)
        {
            var proxy = (DataContextProxy)element.GetValue(DataContextProxyProperty);
            if (proxy == null)
            {
                return null;
            }
            return proxy.DataContext;
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

        private static void OnDataContextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var freezable = (Freezable)o;
            var oldProxy = e.OldValue as DataContextProxy;
            if (oldProxy != null)
            {
                freezable.RemoveInheritanceContextWithReflection(oldProxy, FrameworkElement.DataContextProperty); 
            }
            var newProxy  = e.NewValue as DataContextProxy;
            if (newProxy != null)
            {
                freezable.AddInheritanceContextWithReflection(newProxy, FrameworkElement.DataContextProperty);
            }
        }

        private class DataContextProxy : FrameworkElement
        {
        }
    }
}
