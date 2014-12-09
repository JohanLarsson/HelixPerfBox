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

    public static class ItemCollectionExt
    {
        private static readonly MethodInfo SetItemsSourceMethod = typeof(ItemCollection).GetMethod("SetItemsSource", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo ClearItemsSourceMethod = typeof(ItemCollection).GetMethod("ClearItemsSource", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly ConstructorInfo Constructor = typeof(ItemCollection).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                                                                                    .Single(x => x.GetParameters().Count() == 1 && 
                                                                                                 x.GetParameters().Single().ParameterType == typeof(DependencyObject));

        public static ItemCollection Create(DependencyObject owner)
        {
            var itemCollection = (ItemCollection)Constructor.Invoke(new object[] { owner });
            return itemCollection;
        }

        public static void SetItemsSourceWithReflection(this ItemCollection self, IEnumerable newValue, Func<object, object> getSourceItem = null)
        {
            SetItemsSourceMethod.Invoke(self, new object[] { newValue, getSourceItem });
        }

        public static void ClearItemsSourceWithReflection(this ItemCollection self)
        {
            ClearItemsSourceMethod.Invoke(self, null);
        }

        public static void SubscribeToCollectionChangedWithReflection(this ItemCollection self, Action<object, NotifyCollectionChangedEventArgs> handler)
        {
            var eventInfo = self.GetType().GetEvent("CollectionChanged");
            eventInfo.AddEventHandler(self, handler);
        }
    }
}