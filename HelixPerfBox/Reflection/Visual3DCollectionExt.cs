// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Visual3DCollectionExt.cs" company="">
//   
// </copyright>
// <summary>
//   The visual 3 d collection ext.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// The visual 3 d collection ext.
    /// </summary>
    public static class Visual3DCollectionExt
    {
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        /// <returns>
        /// The <see cref="Visual3DCollection"/>.
        /// </returns>
        public static Visual3DCollection Create(Selector3D  owner)
        {
            var ctor = typeof(Visual3DCollection).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
            var col = (Visual3DCollection)ctor.Invoke(new object[] { owner });
            return col;
        }
    }
}
