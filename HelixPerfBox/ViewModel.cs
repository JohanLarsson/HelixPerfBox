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
    using System.Drawing;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Windows.Media.Media3D;

    using HelixPerfBox.Annotations;

    using Brush = System.Windows.Media.Brush;

    /// <summary>
    /// The view model.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Ball> _balls = new ObservableCollection<Ball>();
        private Ball _selectedBall;
        private bool _isBallsVisible = true;
        private int _side = 5;

        private Brush _selectedBrush;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            PropertyChangedEventManager.AddHandler(this, (_, __) => CreateBalls(Side), "Side");
            CreateBalls(5);
            SubVm = new SubVm();
        }

        public SubVm SubVm { get; private set; }

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

        public System.Windows.Media.Brush[] Brushes
        {
            get
            {
                return new[]
                           {
                              System.Windows.Media.Brushes.Black,
                               System.Windows.Media.Brushes.Red,
                               System.Windows.Media.Brushes.Blue,
                               System.Windows.Media.Brushes.Green,
                           };
            }
        }

        public System.Windows.Media.Brush SelectedBrush
        {
            get
            {
                return _selectedBrush;
            }
            set
            {
                if (Equals(value, _selectedBrush))
                {
                    return;
                }
                _selectedBrush = value;
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

    public class SubVm
    {
        public double VmRadius
        {
            get { return 2.2; }
        }
    }
}
