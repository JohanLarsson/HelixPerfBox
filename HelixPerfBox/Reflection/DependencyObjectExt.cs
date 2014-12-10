namespace HelixPerfBox.Reflection
{
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    public static class DependencyObjectExt
    {
        private static readonly MethodInfo ProvideSelfAsInheritanceContextMethod = typeof(DependencyObject).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m=>m.Name =="ProvideSelfAsInheritanceContext");

        public static bool ProvideSelfAsInheritanceContextWithReflection(this DependencyObject self, object value, DependencyProperty dp)
        {
            return (bool)ProvideSelfAsInheritanceContextMethod.Invoke(self, new object[] { value, dp });
        }
    }
}
