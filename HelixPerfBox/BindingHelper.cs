namespace HelixPerfBox
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using Gu.Reactive;

    internal static class BindingHelper
    {
        private static PropertyInfo[] _bindingPropertyInfos = typeof(Binding).GetProperties();

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

        public static void Bind(this DependencyObject target, DependencyProperty targetProperty, object source)
        {
            var binding = new Binding()
            {
                Source = source,
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(target, targetProperty, binding);
        }

        public static IEnumerable<DependencyProperty> DependencyProperties<T>(this T model)
            where T : DependencyObject
        {
            var type = model.GetType();
            var dps = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                          .Where(x => x.FieldType == typeof(DependencyProperty))
                          .Select(x => (DependencyProperty)x.GetValue(null))
                          .ToArray();
            return dps;
        }

        public static IEnumerable<Binding> Bindings<T>(this T model)
            where T : DependencyObject
        {
            var bindings = model.DependencyProperties()
                                .Select(x => BindingOperations.GetBinding(model, x))
                                .Where(b => b != null)
                                .ToArray();
            return bindings;
        }

        public static IEnumerable<BindingExpressionBase> BindingExpressions<T>(this T model)
    where T : DependencyObject
        {
            var bindings = model.DependencyProperties()
                                .Select(x => BindingOperations.GetBindingExpressionBase(model, x))
                                .Where(b => b != null)
                                .ToArray();
            return bindings;
        }

        public static Binding Clone(this Binding binding)
        {
            var clone = new Binding();
            foreach (var info in _bindingPropertyInfos)
            {
                if (typeof(IEnumerable).IsAssignableFrom(info.PropertyType) && !(info.PropertyType == typeof(string)))
                {
                    if (info.Name == "ValidationRules")
                    {
                        foreach (var rule in binding.ValidationRules)
                        {
                            clone.ValidationRules.Add(rule);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException(string.Format("Not handling {0}", info.Name));
                    }
                }
                else
                {
                    var value = info.GetValue(binding);
                    if (value != null)
                    {
                        info.SetValue(clone, value);
                    }
                }
            }
            return clone;
        }

        public static void SetBindings<T>(this T instance, IEnumerable<BindingExpressionBase> bindingExpressions, object source)
            where T : DependencyObject
        {
            foreach (var expression in bindingExpressions)
            {
                var binding = expression.ParentBindingBase as Binding;
                if (binding == null)
                {
                    throw new NotImplementedException("message");
                }
                var bindingClone = binding.Clone();
                if (binding.RelativeSource == null && binding.ElementName == null)
                {
                    bindingClone.Source = source;
                }
                BindingOperations.SetBinding(instance, expression.TargetProperty, bindingClone);
            }
        }
    }
}
