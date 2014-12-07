namespace HelixPerfBox
{
    using System;
    using System.Linq.Expressions;
    using System.Windows;
    using System.Windows.Data;
    using Gu.Reactive;

    internal static class BindingHelper
    {
        internal static void Bind<TSource>(this DependencyObject target,
                                  DependencyProperty targetProperty,
                                  TSource source,
                                  Expression<Func<TSource, object>> prop)
        {
            var binding = new Binding(NameOf.Property(prop))
            {
                Source = source,
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(target, targetProperty, binding);
        }

        internal static void Bind<TSource>(this DependencyObject target,
                          DependencyProperty targetProperty,
                          TSource source,
                          DependencyProperty sourceProperty)
        {
            var binding = new Binding(sourceProperty.Name)
            {
                Source = source,
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(target, targetProperty, binding);
        }
    }
}
