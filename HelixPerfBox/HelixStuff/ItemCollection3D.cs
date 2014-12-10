// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemCollection3D.cs" company="">
//   
// </copyright>
// <summary>
//   The item collection 3 d.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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

    /// <summary>
    /// The item collection 3 d.
    /// </summary>
    public class ItemCollection3D : ICollectionView
    {
        // , IList, ICollection
        /// <summary>
        /// The _parent.
        /// </summary>
        private readonly WeakReference _parent;

        /// <summary>
        /// The _items.
        /// </summary>
        private readonly ObservableCollection<object> _items = new ObservableCollection<object>();

        /// <summary>
        /// The _collection view.
        /// </summary>
        private ICollectionView _collectionView;

        /// <summary>
        /// The _collection view source.
        /// </summary>
        private readonly CollectionViewSource _collectionViewSource = new CollectionViewSource();

        /// <summary>
        /// The _is using items source.
        /// </summary>
        private bool _isUsingItemsSource;

        /// <summary>
        /// The _items source changed descriptor.
        /// </summary>
        private readonly DependencyPropertyDescriptor _itemsSourceChangedDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl3D.ItemsSourceProperty, typeof(ItemsControl3D));

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCollection3D"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        internal ItemCollection3D(ItemsControl3D parent)
        {
            _parent = new WeakReference(parent);
            SetItemsSource(parent.ItemsSource ?? _items);
        }

        /// <summary>
        /// The clear items source.
        /// </summary>
        public void ClearItemsSource()
        {
            SetItemsSource(Enumerable.Empty<object>());
        }

        /// <summary>
        /// The set items source.
        /// </summary>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        public void SetItemsSource(IEnumerable newValue)
        {
            _isUsingItemsSource = true;

            CollectionChangedEventManager.RemoveHandler(_collectionView, OnCollectionChanged);
            CurrentChangingEventManager.RemoveHandler(_collectionView, OnCurrentChanging);
            CurrentChangedEventManager.RemoveHandler(_collectionView, OnCurrentChanged);
            if (newValue != null)
            {
                _collectionView = CollectionViewSource.GetDefaultView(newValue);
                CollectionChangedEventManager.AddHandler(_collectionView, OnCollectionChanged);
                CurrentChangingEventManager.AddHandler(_collectionView, OnCurrentChanging);
                CurrentChangedEventManager.AddHandler(_collectionView, OnCurrentChanged);
            }
            else
            {
                _collectionView = new CollectionView(Enumerable.Empty<object>());
            }
            _collectionView.Refresh();
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        internal ItemsControl3D Parent
        {
            get
            {
                return (ItemsControl3D)_parent.Target;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is using items source.
        /// </summary>
        internal bool IsUsingItemsSource
        {
            get
            {
                return _isUsingItemsSource;
            }
        }

        // #region ICollection & ILIst

        // bool ICollection.IsSynchronized
        // {
        // get { return false; }
        // }

        // object ICollection.SyncRoot
        // {
        // get
        // {
        // if (this.IsUsingItemsSource)
        // throw new NotSupportedException("ItemCollectionShouldUseInnerSyncRoot");
        // else
        // return ((ICollection)_items).SyncRoot;
        // }
        // }

        // bool IList.IsFixedSize
        // {
        // get { return this.IsUsingItemsSource; }
        // }

        // bool IList.IsReadOnly
        // {
        // get { return this.IsUsingItemsSource; }
        // }

        // public int Add(object newItem)
        // {
        // _items.Add(newItem);
        // Parent.SetValue(ItemsControl3D.HasItemsPropertyKey, true);
        // return _collectionView.IndexOf(newItem);
        // }

        // public void Clear()
        // {
        // _items.Clear();
        // this.Parent.ClearValue(ItemsControl3D.HasItemsPropertyKey);
        // }

        // public void CopyTo(Array array, int index)
        // {
        // if (array == null)
        // throw new ArgumentNullException("array");
        // if (array.Rank > 1)
        // throw new ArgumentException("array.Rank > 1");
        // if (index < 0)
        // throw new ArgumentOutOfRangeException("index");
        // _collectionView.Cast<object>().ToArray().CopyTo(array, index);
        // }

        // public int Count { get { return _collectionView.Count(); } }

        // public int IndexOf(object item)
        // {
        // return _collectionView.IndexOf(item);
        // }

        // public object GetItemAt(int index)
        // {
        // if (index < 0)
        // throw new ArgumentOutOfRangeException("index");
        // if (index >= this._collectionView.Count)
        // throw new ArgumentOutOfRangeException("index");
        // else
        // return this._collectionView.GetItemAt(index);
        // }

        // public void Insert(int insertIndex, object insertItem)
        // {
        // if (insertIndex == 0)
        // {
        // _items.Insert(insertIndex, insertItem);
        // }
        // else
        // {
        // var indexOf = _items.IndexOf(_collectionView.GetItemAt(insertIndex));
        // _items.Insert(indexOf, insertItem);
        // }
        // this.Parent.SetValue(ItemsControl3D.HasItemsPropertyKey, true);
        // }

        // public void Remove(object removeItem)
        // {
        // _items.Remove(removeItem);
        // if (!this.IsEmpty)
        // return;
        // this.Parent.ClearValue(ItemsControl3D.HasItemsPropertyKey);
        // }

        // public void RemoveAt(int removeIndex)
        // {
        // _collectionView.RemoveAt(removeIndex);
        // if (!this.IsEmpty)
        // return;
        // this.Parent.ClearValue(ItemsControl3D.HasItemsPropertyKey);
        // }

        // public object this[int index]
        // {
        // get
        // {
        // return _collectionView.GetItemAt(index);
        // }
        // set
        // {
        // var itemAt = _collectionView.GetItemAt(index);
        // var indexOf = _items.IndexOf(itemAt);
        // _items[indexOf] = value; // Not efficient nor nice here
        // }
        // }

        // #endregion ICollection & ILIst
        #region ICollectionView

        /// <summary>
        /// The collection changed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// The current changing.
        /// </summary>
        public event CurrentChangingEventHandler CurrentChanging;

        /// <summary>
        /// The current changed.
        /// </summary>
        public event EventHandler CurrentChanged;

        /// <summary>
        /// The on collection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="notifyCollectionChangedEventArgs">
        /// The notify collection changed event args.
        /// </param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            OnCollectionChanged(notifyCollectionChangedEventArgs);
        }

        /// <summary>
        /// The on collection changed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// The on current changing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="currentChangingEventArgs">
        /// The current changing event args.
        /// </param>
        private void OnCurrentChanging(object sender, CurrentChangingEventArgs currentChangingEventArgs)
        {
            OnCurrentChanging(currentChangingEventArgs);
        }

        /// <summary>
        /// The on current changing.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnCurrentChanging(CurrentChangingEventArgs e)
        {
            var handler = CurrentChanging;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// The on current changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void OnCurrentChanged(object sender, EventArgs eventArgs)
        {
            OnCurrentChanged();
        }

        /// <summary>
        /// The on current changed.
        /// </summary>
        protected virtual void OnCurrentChanged()
        {
            var handler = CurrentChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public CultureInfo Culture
        {
            get { return _collectionView.Culture; }
            set { _collectionView.Culture = value; }
        }

        /// <summary>
        /// Gets the source collection.
        /// </summary>
        public IEnumerable SourceCollection { get { return _items; } }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        public Predicate<object> Filter
        {
            get { return _collectionView.Filter; }
            set { _collectionView.Filter = value; }
        }

        /// <summary>
        /// Gets a value indicating whether can filter.
        /// </summary>
        public bool CanFilter { get { return _collectionView.CanFilter; } }

        /// <summary>
        /// Gets the sort descriptions.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public SortDescriptionCollection SortDescriptions { get { throw new InvalidOperationException("Can a list of 3D items be sorted?"); } }

        /// <summary>
        /// Gets a value indicating whether can sort.
        /// </summary>
        public bool CanSort { get { return false; } }

        /// <summary>
        /// Gets a value indicating whether can group.
        /// </summary>
        public bool CanGroup { get { return false; } }

        /// <summary>
        /// Gets the group descriptions.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public ObservableCollection<GroupDescription> GroupDescriptions { get { throw new InvalidOperationException("Can a list of 3D items be grouped?"); } }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public ReadOnlyObservableCollection<object> Groups { get { throw new InvalidOperationException("Can a list of 3D items be grouped?"); } }

        /// <summary>
        /// Gets a value indicating whether is empty.
        /// </summary>
        public bool IsEmpty { get { return _collectionView.IsEmpty; } }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        public object CurrentItem { get { return _collectionView.CurrentItem; } }

        /// <summary>
        /// Gets the current position.
        /// </summary>
        public int CurrentPosition { get { return _collectionView.CurrentPosition; } }

        /// <summary>
        /// Gets a value indicating whether is current after last.
        /// </summary>
        public bool IsCurrentAfterLast { get { return _collectionView.IsCurrentAfterLast; } }

        /// <summary>
        /// Gets a value indicating whether is current before first.
        /// </summary>
        public bool IsCurrentBeforeFirst { get { return _collectionView.IsCurrentBeforeFirst; } }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return _collectionView.GetEnumerator();
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Contains(object item)
        {
            return _collectionView.Contains(item);
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        public void Refresh()
        {
            _collectionView.Refresh();
        }

        /// <summary>
        /// The defer refresh.
        /// </summary>
        /// <returns>
        /// The <see cref="IDisposable"/>.
        /// </returns>
        public IDisposable DeferRefresh()
        {
            return _collectionView.DeferRefresh();
        }

        /// <summary>
        /// The move current to first.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool MoveCurrentToFirst()
        {
            return _collectionView.MoveCurrentToFirst();
        }

        /// <summary>
        /// The move current to last.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool MoveCurrentToLast()
        {
            return _collectionView.MoveCurrentToLast();
        }

        /// <summary>
        /// The move current to next.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool MoveCurrentToNext()
        {
            return _collectionView.MoveCurrentToNext();
        }

        /// <summary>
        /// The move current to previous.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool MoveCurrentToPrevious()
        {
            return _collectionView.MoveCurrentToPrevious();
        }

        /// <summary>
        /// The move current to.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool MoveCurrentTo(object item)
        {
            return _collectionView.MoveCurrentTo(item);
        }

        /// <summary>
        /// The move current to position.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool MoveCurrentToPosition(int position)
        {
            return _collectionView.MoveCurrentToPosition(position);
        }

        #endregion  ICollectionView
    }
}
