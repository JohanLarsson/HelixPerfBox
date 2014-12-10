namespace HelixPerfBox
{
    using System.Reflection;
    using System.Windows;

    public static class FrameworkElementExt
    {
        public static readonly EventPrivateKey DataContextChangedKey = (EventPrivateKey)typeof(FrameworkElement).GetField("DataContextChangedKey", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }
}
