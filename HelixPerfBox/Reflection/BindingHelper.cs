namespace HelixPerfBox
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Gu.Reactive;

    internal static class BindingHelper
    {
        public const string RelativeSource = "RelativeSource";
        public const string ElementName = "ElementName";
        public const string Source = "Source";
        public const string Path = "Path";
        public const string ValidationRules = "ValidationRules";
        public const string Bindings_ = "Bindings";

        private static readonly MethodInfo GetSourceItemMethod = typeof(BindingExpressionBase).GetMethod(
            "GetSourceItem",
            BindingFlags.Instance | BindingFlags.NonPublic);

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

        public static IEnumerable<BindingExpressionBase> BindingExpressions<T>(this T model) where T : DependencyObject
        {
            var bindings = model.DependencyProperties()
                                .Select(x => BindingOperations.GetBindingExpressionBase(model, x))
                                .Where(b => b != null)
                                .ToArray();
            return bindings;
        }

        public static T Clone<T>(this T binding)
            where T : BindingBase
        {
            var type = binding.GetType();
            var clone = (T)Activator.CreateInstance(type);

            var properties = type.GetProperties();
            foreach (var info in properties)
            {
                if (!info.CanWrite)
                {
                    if (info.Name == ValidationRules)
                    {
                        var sourceRules = (Collection<ValidationRule>)info.GetValue(binding);
                        var cloneRules = (Collection<ValidationRule>)info.GetValue(clone);
                        foreach (var rule in sourceRules)
                        {
                            cloneRules.Add(rule);
                        }
                    }
                    else if (info.Name == Bindings_)
                    {
                        var sourceBindings = (Collection<BindingBase>)info.GetValue(binding);
                        var cloneBindings = (Collection<BindingBase>)info.GetValue(clone);
                        foreach (var b in sourceBindings)
                        {
                            var bindingBase = b.Clone();
                            cloneBindings.Add(bindingBase);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException(string.Format("Not handling {0}.{1}", type.Name, info.Name));
                    }
                }
                else
                {
                    var value = info.GetValue(binding);
                    if (value != info.GetValue(clone))
                    {
                        info.SetValue(clone, value);
                    }
                }
            }
            return clone;
        }

        public static void Rebind(this Binding binding, object source)
        {
            var hasRelativesource = binding.HasValue(RelativeSource);
            var hasElementname = binding.HasValue(ElementName);
            if (!(hasRelativesource || hasElementname))
            {
                var type = binding.GetType();
                var sourceProp = type.GetProperty(Source);
                sourceProp.SetValue(binding, source);
            }
        }

        public static void Rebind(this MultiBinding multiBinding, object source)
        {
            foreach (var binding in multiBinding.Bindings)
            {
                binding.Rebind(source);
            }
        }
        public static void Rebind(this BindingBase bindingBase, object source)
        {
            var binding = bindingBase as Binding;
            if (binding != null)
            {
                binding.Rebind(source);
                return;
            }
            var multiBinding = bindingBase as MultiBinding;
            if (multiBinding != null)
            {
                multiBinding.Rebind(source);
                return;
            }
            throw new NotImplementedException(string.Format("Not handling rebind of {0}", bindingBase.GetType().Name));
        }

        public static void SetBindings<T>(this T instance, IEnumerable<BindingExpressionBase> bindingExpressions, object source)
            where T : DependencyObject
        {
            foreach (var expression in bindingExpressions)
            {
                var bindingBase = expression.ParentBindingBase;
                var clone = bindingBase.Clone();
                clone.Rebind(source);
                BindingOperations.SetBinding(instance, expression.TargetProperty, clone);
            }
        }

        public static bool HasValue<T>(this T binding, string propName) where T : BindingBase
        {
            return binding.GetValueOrDefault(propName) != null;
        }

        public static object GetValueOrDefault<T>(this T binding, string propName) where T : BindingBase
        {
            var type = binding.GetType();
            var property = type.GetProperty(propName);
            if (property == null)
            {
                return null;
            }
            return property.GetValue(binding);
        }

        public static object GetSourceItemWithReflection(this BindingExpressionBase self, object newValue)
        {
            var invoke = GetSourceItemMethod.Invoke(self, new[] { newValue });
            return invoke;
        }
    }
}
