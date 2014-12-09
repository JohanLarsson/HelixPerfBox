namespace HelixPerfBox.Reflection
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Using reflection to access http://referencesource.microsoft.com/#PresentationFramework/Framework/MS/Internal/Helper.cs,077167ef4ff9daa6
    /// No idea what this does. Hack to use ItemCollection temp
    /// </summary>
    public class Helper
    {
        private static readonly Type HelperType = Type.GetType("MS.Internal.Helper");
        private static readonly MethodInfo ReadItemValueMethod = HelperType.GetMethod("ReadItemValue", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly MethodInfo StoreItemValueMethod = HelperType.GetMethod("StoreItemValue", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly MethodInfo ClearItemValueMethod = HelperType.GetMethod("ClearItemValue", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly MethodInfo ClearItemValueStorageMethod = HelperType.GetMethod("ClearItemValueStorage", BindingFlags.Static | BindingFlags.NonPublic);
        public static object ReadItemValue(ItemsControl3D itemsControl3D, object item, int globalIndex)
        {
            return ReadItemValueMethod.Invoke(null, new[] { itemsControl3D, item, globalIndex });
        }

        public static void StoreItemValue(ItemsControl3D itemsControl3D, object item, int globalIndex, object value)
        {
            StoreItemValueMethod.Invoke(null, new[] { itemsControl3D, item, globalIndex, value });
        }

        public static void ClearItemValue(ItemsControl3D itemsControl3D, object item, int globalIndex)
        {
            ClearItemValueMethod.Invoke(null, new[] { itemsControl3D, item, globalIndex });
        }

        public static void ClearItemValueStorage(ItemsControl3D itemsControl3D, int[] ints)
        {
            ClearItemValueStorageMethod.Invoke(null, new object[] { itemsControl3D, ints });
        }

        public static void ClearItemValueStorage(ItemsControl3D itemsControl3D)
        {
            ClearItemValueStorageMethod.Invoke(null, new object[] { itemsControl3D });
        }
    }
}
