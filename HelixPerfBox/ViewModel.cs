// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HelixPerfBox
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Windows.Media.Media3D;

    using HelixPerfBox.Annotations;

    /// <summary>
    /// The view model.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The _balls.
        /// </summary>
        private readonly ObservableCollection<Ball> _balls = new ObservableCollection<Ball>();

        /// <summary>
        /// The _selected ball.
        /// </summary>
        private Ball _selectedBall;

        /// <summary>
        /// The _is balls visible.
        /// </summary>
        private bool _isBallsVisible = true;

        /// <summary>
        /// The _side.
        /// </summary>
        private int _side = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            PropertyChangedEventManager.AddHandler(this, (_, __) => CreateBalls(Side), "Side");
            CreateBalls(5);
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the balls.
        /// </summary>
        public ObservableCollection<Ball> Balls
        {
            get { return _balls; }
        }

        /// <summary>
        /// Gets or sets the selected ball.
        /// </summary>
        public Ball SelectedBall
        {
            get { return _selectedBall; }
            set
            {
                if (Equals(value, _selectedBall))
                {
                    return;
                }

                _selectedBall = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is balls visible.
        /// </summary>
        public bool IsBallsVisible
        {
            get { return _isBallsVisible; }
            set
            {
                if (value.Equals(_isBallsVisible))
                {
                    return;
                }

                _isBallsVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is 0.
        /// </summary>
        public bool Is0
        {
            get { return _side == 0; }
            set { Side = 0; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is 25.
        /// </summary>
        public bool Is25
        {
            get { return _side == 5; }
            set { Side = 5; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is 100.
        /// </summary>
        public bool Is100
        {
            get { return _side == 10; }
            set { Side = 10; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is 400.
        /// </summary>
        public bool Is400
        {
            get { return _side == 20; }
            set { Side = 20; }
        }

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        public int Side
        {
            get { return _side; }
            set
            {
                if (value == _side)
                {
                    return;
                }

                _side = value;
                OnPropertyChanged();

                OnPropertyChanged(() => Is0);
                OnPropertyChanged(() => Is25);
                OnPropertyChanged(() => Is100);
                OnPropertyChanged(() => Is400);
            }
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Calls nameof internally
        /// </summary>
        /// <param name="propety">
        /// </param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propety)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(NameOf.Property(propety)));
            }
        }

        /// <summary>
        /// The create balls.
        /// </summary>
        /// <param name="side">
        /// The side.
        /// </param>
        private void CreateBalls(int side)
        {
            _balls.Clear();
            for (int x = 0; x < side; x++)
            {
                for (int y = 0; y < side; y++)
                {
                    _balls.Add(new Ball(new Point3D(x, y, 0), 0.3));
                }
            }
        }
    }
}
