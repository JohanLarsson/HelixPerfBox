namespace HelixPerfBox
{
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media.Media3D;

    public static class Visual3DCollectionExt
    {
        public static Visual3DCollection Create(Selector3D  owner)
        {
            var ctor = typeof(Visual3DCollection).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
            var col = (Visual3DCollection)ctor.Invoke(new object[] { owner });
            return col;
        }
    }
}
