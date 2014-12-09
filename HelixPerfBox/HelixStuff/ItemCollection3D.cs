namespace HelixPerfBox
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    public class ItemCollection3D : ICollectionView //, IList, ICollection
    {
        private readonly WeakReference _parent;
        private readonly ObservableCollection<object> _items = new ObservableCollection<object>();
        private ICollectionView _collectionView;
        private readonly CollectionViewSource _collectionViewSource = new CollectionViewSource();
        private bool _isUsingItemsSource;

        private readonly DependencyPropertyDescriptor _itemsSourceChangedDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl3D.ItemsSourceProperty, typeof(ItemsControl3D));
        internal ItemCollection3D(ItemsControl3D parent)
        {
            _parent = new WeakReference(parent);
            this.SetItemsSource(parent.ItemsSource ?? _items);
        }

        public void ClearItemsSource()
        {
            this.SetItemsSource(Enumerable.Empty<object>());
        }
       
        public void SetItemsSource(IEnumerable newValue)
        {
            _isUsingItemsSource = true;

            CollectionChangedEventManager.RemoveHandler(_collectionView, OnCollectionChanged);
            CurrentChangingEventManager.RemoveHandler(_collectionView, OnCurrentChanging);
            CurrentChangedEventManager.RemoveHandler(_collectionView, OnCurrentChanged);
            
            _collectionViewSource.Source = newValue;
            _collectionView = _collectionViewSource.View;
            CollectionChangedEventManager.AddHandler(_collectionView, OnCollectionChanged);
            CurrentChangingEventManager.AddHandler(_collectionView, OnCurrentChanging);
            CurrentChangedEventManager.AddHandler(_collectionView, OnCurrentChanged);
            _collectionView.Refresh();
        }

        internal ItemsControl3D Parent
        {
            get
            {
                return (ItemsControl3D)this._parent.Target;
            }
        }

        internal bool IsUsingItemsSource
        {
            get
            {
                return _isUsingItemsSource;
            }
        }

        //#region ICollection & ILIst

        //bool ICollection.IsSynchronized
        //{
        //    get { return false; }
        //}

        //object ICollection.SyncRoot
        //{
        //    get
        //    {
        //        if (this.IsUsingItemsSource)
        //            throw new NotSupportedException("ItemCollectionShouldUseInnerSyncRoot");
        //        else
        //            return ((ICollection)_items).SyncRoot;
        //    }
        //}

        //bool IList.IsFixedSize
        //{
        //    get { return this.IsUsingItemsSource; }
        //}

        //bool IList.IsReadOnly
        //{
        //    get { return this.IsUsingItemsSource; }
        //}

        //public int Add(object newItem)
        //{
        //    _items.Add(newItem);
        //    Parent.SetValue(ItemsControl3D.HasItemsPropertyKey, true);
        //    return _collectionView.IndexOf(newItem);
        //}

        //public void Clear()
        //{
        //    _items.Clear();
        //    this.Parent.ClearValue(ItemsControl3D.HasItemsPropertyKey);
        //}

        //public void CopyTo(Array array, int index)
        //{
        //    if (array == null)
        //        throw new ArgumentNullException("array");
        //    if (array.Rank > 1)
        //        throw new ArgumentException("array.Rank > 1");
        //    if (index < 0)
        //        throw new ArgumentOutOfRangeException("index");
        //    _collectionView.Cast<object>().ToArray().CopyTo(array, index);
        //}

        //public int Count { get { return _collectionView.Count(); } }

        //public int IndexOf(object item)
        //{
        //    return _collectionView.IndexOf(item);
        //}

        //public object GetItemAt(int index)
        //{
        //    if (index < 0)
        //        throw new ArgumentOutOfRangeException("index");
        //    if (index >= this._collectionView.Count)
        //        throw new ArgumentOutOfRangeException("index");
        //    else
        //        return this._collectionView.GetItemAt(index);
        //}

        //public void Insert(int insertIndex, object insertItem)
        //{
        //    if (insertIndex == 0)
        //    {
        //        _items.Insert(insertIndex, insertItem);
        //    }
        //    else
        //    {
        //        var indexOf = _items.IndexOf(_collectionView.GetItemAt(insertIndex));
        //        _items.Insert(indexOf, insertItem);
        //    }
        //    this.Parent.SetValue(ItemsControl3D.HasItemsPropertyKey, true);
        //}

        //public void Remove(object removeItem)
        //{
        //    _items.Remove(removeItem);
        //    if (!this.IsEmpty)
        //        return;
        //    this.Parent.ClearValue(ItemsControl3D.HasItemsPropertyKey);
        //}

        //public void RemoveAt(int removeIndex)
        //{
        //    _collectionView.RemoveAt(removeIndex);
        //    if (!this.IsEmpty)
        //        return;
        //    this.Parent.ClearValue(ItemsControl3D.HasItemsPropertyKey);
        //}

        //public object this[int index]
        //{
        //    get
        //    {
        //        return _collectionView.GetItemAt(index);
        //    }
        //    set
        //    {
        //        var itemAt = _collectionView.GetItemAt(index);
        //        var indexOf = _items.IndexOf(itemAt);
        //        _items[indexOf] = value; // Not efficient nor nice here
        //    }
        //}

        //#endregion ICollection & ILIst

        #region ICollectionView

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event CurrentChangingEventHandler CurrentChanging;

        public event EventHandler CurrentChanged;

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            this.OnCollectionChanged(notifyCollectionChangedEventArgs);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnCurrentChanging(object sender, CurrentChangingEventArgs currentChangingEventArgs)
        {
            this.OnCurrentChanging(currentChangingEventArgs);
        }

        protected virtual void OnCurrentChanging(CurrentChangingEventArgs e)
        {
            var handler = this.CurrentChanging;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnCurrentChanged(object sender, EventArgs eventArgs)
        {
            this.OnCurrentChanged();
        }

        protected virtual void OnCurrentChanged()
        {
            var handler = this.CurrentChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public CultureInfo Culture
        {
            get { return _collectionView.Culture; }
            set { _collectionView.Culture = value; }
        }

        public IEnumerable SourceCollection { get { return _items; } }

        public Predicate<object> Filter
        {
            get { return _collectionView.Filter; }
            set { _collectionView.Filter = value; }
        }

        public bool CanFilter { get { return _collectionView.CanFilter; } }

        public SortDescriptionCollection SortDescriptions { get { throw new InvalidOperationException("Can a list of 3D items be sorted?"); } }

        public bool CanSort { get { return false; } }

        public bool CanGroup { get { return false; } }

        public ObservableCollection<GroupDescription> GroupDescriptions { get { throw new InvalidOperationException("Can a list of 3D items be grouped?"); } }

        public ReadOnlyObservableCollection<object> Groups { get { throw new InvalidOperationException("Can a list of 3D items be grouped?"); } }

        public bool IsEmpty { get { return _collectionView.IsEmpty; } }

        public object CurrentItem { get { return _collectionView.CurrentItem; } }

        public int CurrentPosition { get { return _collectionView.CurrentPosition; } }

        public bool IsCurrentAfterLast { get { return _collectionView.IsCurrentAfterLast; } }

        public bool IsCurrentBeforeFirst { get { return _collectionView.IsCurrentBeforeFirst; } }

        public IEnumerator GetEnumerator()
        {
            return _collectionView.GetEnumerator();
        }

        public bool Contains(object item)
        {
            return _collectionView.Contains(item);
        }

        public void Refresh()
        {
            _collectionView.Refresh();
        }

        public IDisposable DeferRefresh()
        {
            return _collectionView.DeferRefresh();
        }

        public bool MoveCurrentToFirst()
        {
            return _collectionView.MoveCurrentToFirst();
        }

        public bool MoveCurrentToLast()
        {
            return _collectionView.MoveCurrentToLast();
        }

        public bool MoveCurrentToNext()
        {
            return _collectionView.MoveCurrentToNext();
        }

        public bool MoveCurrentToPrevious()
        {
            return _collectionView.MoveCurrentToPrevious();
        }

        public bool MoveCurrentTo(object item)
        {
            return _collectionView.MoveCurrentTo(item);
        }

        public bool MoveCurrentToPosition(int position)
        {
            return _collectionView.MoveCurrentToPosition(position);
        }

        #endregion  ICollectionView
    }
}
