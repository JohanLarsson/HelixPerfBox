namespace HelixPerfBox
{
    using System;
    using System.Collections;
    using System.Windows;

    /// <summary>
    /// The data context proxy.
    /// </summary>
    public class DataContextProxy : FrameworkElement
    {
        private readonly WeakReference _child = new WeakReference(null);
        public void AddLogicalChild(DependencyObject child)
        {
            _child.Target = child;
            AddLogicalChild((object)child);
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                var child = _child.Target;
                if (child != null)
                {
                    yield return child;
                }
            }
        }
    }
}