// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="">
//   
// </copyright>
// <summary>
//   Using reflection to access http://referencesource.microsoft.com/#PresentationFramework/Framework/MS/Internal/Helper.cs,077167ef4ff9daa6
//   No idea what this does. Hack to use ItemCollection temp
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
        /// <summary>
        /// The helper type.
        /// </summary>
        private static readonly Type HelperType = Type.GetType("MS.Internal.Helper");

        /// <summary>
        /// The read item value method.
        /// </summary>
        private static readonly MethodInfo ReadItemValueMethod = HelperType.GetMethod("ReadItemValue", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// The store item value method.
        /// </summary>
        private static readonly MethodInfo StoreItemValueMethod = HelperType.GetMethod("StoreItemValue", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// The clear item value method.
        /// </summary>
        private static readonly MethodInfo ClearItemValueMethod = HelperType.GetMethod("ClearItemValue", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// The clear item value storage method.
        /// </summary>
        private static readonly MethodInfo ClearItemValueStorageMethod = HelperType.GetMethod("ClearItemValueStorage", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// The read item value.
        /// </summary>
        /// <param name="itemsControl3D">
        /// The items control 3 d.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="globalIndex">
        /// The global index.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ReadItemValue(ItemsControl3D itemsControl3D, object item, int globalIndex)
        {
            return ReadItemValueMethod.Invoke(null, new[] { itemsControl3D, item, globalIndex });
        }

        /// <summary>
        /// The store item value.
        /// </summary>
        /// <param name="itemsControl3D">
        /// The items control 3 d.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="globalIndex">
        /// The global index.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void StoreItemValue(ItemsControl3D itemsControl3D, object item, int globalIndex, object value)
        {
            StoreItemValueMethod.Invoke(null, new[] { itemsControl3D, item, globalIndex, value });
        }

        /// <summary>
        /// The clear item value.
        /// </summary>
        /// <param name="itemsControl3D">
        /// The items control 3 d.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="globalIndex">
        /// The global index.
        /// </param>
        public static void ClearItemValue(ItemsControl3D itemsControl3D, object item, int globalIndex)
        {
            ClearItemValueMethod.Invoke(null, new[] { itemsControl3D, item, globalIndex });
        }

        /// <summary>
        /// The clear item value storage.
        /// </summary>
        /// <param name="itemsControl3D">
        /// The items control 3 d.
        /// </param>
        /// <param name="ints">
        /// The ints.
        /// </param>
        public static void ClearItemValueStorage(ItemsControl3D itemsControl3D, int[] ints)
        {
            ClearItemValueStorageMethod.Invoke(null, new object[] { itemsControl3D, ints });
        }

        /// <summary>
        /// The clear item value storage.
        /// </summary>
        /// <param name="itemsControl3D">
        /// The items control 3 d.
        /// </param>
        public static void ClearItemValueStorage(ItemsControl3D itemsControl3D)
        {
            ClearItemValueStorageMethod.Invoke(null, new object[] { itemsControl3D });
        }
    }
}
