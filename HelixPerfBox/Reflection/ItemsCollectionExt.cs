// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsCollectionExt.cs" company="">
//   
// </copyright>
// <summary>
//   The item collection ext.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The item collection ext.
    /// </summary>
    public static class ItemCollectionExt
    {
        /// <summary>
        /// The set items source method.
        /// </summary>
        private static readonly MethodInfo SetItemsSourceMethod = typeof(ItemCollection).GetMethod("SetItemsSource", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// The clear items source method.
        /// </summary>
        private static readonly MethodInfo ClearItemsSourceMethod = typeof(ItemCollection).GetMethod("ClearItemsSource", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// The constructor.
        /// </summary>
        private static readonly ConstructorInfo Constructor = typeof(ItemCollection).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                                                                                    .Single(x => x.GetParameters().Count() == 1 && 
                                                                                                 x.GetParameters().Single().ParameterType == typeof(DependencyObject));

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        /// <returns>
        /// The <see cref="ItemCollection"/>.
        /// </returns>
        public static ItemCollection Create(DependencyObject owner)
        {
            var itemCollection = (ItemCollection)Constructor.Invoke(new object[] { owner });
            return itemCollection;
        }

        /// <summary>
        /// The set items source with reflection.
        /// </summary>
        /// <param name="self">
        /// The self.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        /// <param name="getSourceItem">
        /// The get source item.
        /// </param>
        public static void SetItemsSourceWithReflection(this ItemCollection self, IEnumerable newValue, Func<object, object> getSourceItem = null)
        {
            SetItemsSourceMethod.Invoke(self, new object[] { newValue, getSourceItem });
        }

        /// <summary>
        /// The clear items source with reflection.
        /// </summary>
        /// <param name="self">
        /// The self.
        /// </param>
        public static void ClearItemsSourceWithReflection(this ItemCollection self)
        {
            ClearItemsSourceMethod.Invoke(self, null);
        }

        /// <summary>
        /// The subscribe to collection changed with reflection.
        /// </summary>
        /// <param name="self">
        /// The self.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        public static void SubscribeToCollectionChangedWithReflection(this ItemCollection self, Action<object, NotifyCollectionChangedEventArgs> handler)
        {
            var eventInfo = self.GetType().GetEvent("CollectionChanged");
            eventInfo.AddEventHandler(self, handler);
        }
    }
}