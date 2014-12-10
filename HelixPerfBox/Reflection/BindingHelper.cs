// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindingHelper.cs" company="">
//   
// </copyright>
// <summary>
//   The binding helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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

    /// <summary>
    /// The binding helper.
    /// </summary>
    internal static class BindingHelper
    {
        /// <summary>
        /// The relative source.
        /// </summary>
        public const string RelativeSource = "RelativeSource";

        /// <summary>
        /// The element name.
        /// </summary>
        public const string ElementName = "ElementName";

        /// <summary>
        /// The source.
        /// </summary>
        public const string Source = "Source";

        /// <summary>
        /// The path.
        /// </summary>
        public const string Path = "Path";

        /// <summary>
        /// The validation rules.
        /// </summary>
        public const string ValidationRules = "ValidationRules";

        /// <summary>
        /// The bindings_.
        /// </summary>
        public const string Bindings_ = "Bindings";

        /// <summary>
        /// The get source item method.
        /// </summary>
        private static readonly MethodInfo GetSourceItemMethod = typeof(BindingExpressionBase).GetMethod(
            "GetSourceItem", 
            BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// The bind.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="targetProperty">
        /// The target property.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="prop">
        /// The prop.
        /// </param>
        /// <typeparam name="TSource">
        /// </typeparam>
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

        /// <summary>
        /// The bind.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="targetProperty">
        /// The target property.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="sourceProperty">
        /// The source property.
        /// </param>
        /// <typeparam name="TSource">
        /// </typeparam>
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

        /// <summary>
        /// The bind.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="targetProperty">
        /// The target property.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        public static void Bind(this DependencyObject target, DependencyProperty targetProperty, object source)
        {
            var binding = new Binding()
            {
                Source = source, 
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(target, targetProperty, binding);
        }

        /// <summary>
        /// The dependency properties.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
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

        /// <summary>
        /// The bindings.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<Binding> Bindings<T>(this T model)
            where T : DependencyObject
        {
            var bindings = model.DependencyProperties()
                                .Select(x => BindingOperations.GetBinding(model, x))
                                .Where(b => b != null)
                                .ToArray();
            return bindings;
        }

        /// <summary>
        /// The binding expressions.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<BindingExpressionBase> BindingExpressions<T>(this T model) where T : DependencyObject
        {
            var bindings = model.DependencyProperties()
                                .Select(x => BindingOperations.GetBindingExpressionBase(model, x))
                                .Where(b => b != null)
                                .ToArray();
            return bindings;
        }

        /// <summary>
        /// The clone.
        /// </summary>
        /// <param name="binding">
        /// The binding.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
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

        /// <summary>
        /// The rebind.
        /// </summary>
        /// <param name="binding">
        /// The binding.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
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

        /// <summary>
        /// The rebind.
        /// </summary>
        /// <param name="multiBinding">
        /// The multi binding.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        public static void Rebind(this MultiBinding multiBinding, object source)
        {
            foreach (var binding in multiBinding.Bindings)
            {
                binding.Rebind(source);
            }
        }

        /// <summary>
        /// The rebind.
        /// </summary>
        /// <param name="bindingBase">
        /// The binding base.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
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

        /// <summary>
        /// The set bindings.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="bindingExpressions">
        /// The binding expressions.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
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

        /// <summary>
        /// The has value.
        /// </summary>
        /// <param name="binding">
        /// The binding.
        /// </param>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool HasValue<T>(this T binding, string propName) where T : BindingBase
        {
            return binding.GetValueOrDefault(propName) != null;
        }

        /// <summary>
        /// The get value or default.
        /// </summary>
        /// <param name="binding">
        /// The binding.
        /// </param>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
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

        /// <summary>
        /// The get source item with reflection.
        /// </summary>
        /// <param name="self">
        /// The self.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object GetSourceItemWithReflection(this BindingExpressionBase self, object newValue)
        {
            var invoke = GetSourceItemMethod.Invoke(self, new[] { newValue });
            return invoke;
        }
    }

    /// <summary>
    /// The name of.
    /// </summary>
    internal class NameOf
    {
        /// <summary>
        /// The property.
        /// </summary>
        /// <param name="prop">
        /// The prop.
        /// </param>
        /// <typeparam name="TSource">
        /// </typeparam>
        /// <typeparam name="TProp">
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Property<TSource, TProp>(Expression<Func<TSource, TProp>> prop)
        {
            var memberExpression = (MemberExpression) prop.Body;
            return memberExpression.Member.Name;
        }

        /// <summary>
        /// The property.
        /// </summary>
        /// <param name="prop">
        /// The prop.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Property<T>(Expression<Func<T>> prop)
        {
            var memberExpression = (MemberExpression)prop.Body;
            return memberExpression.Member.Name;
        }
    }
}
