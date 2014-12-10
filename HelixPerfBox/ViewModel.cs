namespace HelixPerfBox
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Windows.Media.Media3D;
    using Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Ball> _balls = new ObservableCollection<Ball>();

        private Ball _selectedBall;
        private bool _isBallsVisible = true;

        private int _side = 5;

        public ViewModel()
        {
            PropertyChangedEventManager.AddHandler(this, (_, __) => CreateBalls(Side), "Side");
            CreateBalls(5);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Ball> Balls
        {
            get { return _balls; }
        }

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

        public bool Is0
        {
            get { return _side == 0; }
            set { Side = 0; }
        }

        public bool Is25
        {
            get { return _side == 5; }
            set { Side = 5; }
        }

        public bool Is100
        {
            get { return _side == 10; }
            set { Side = 10; }
        }

        public bool Is400
        {
            get { return _side == 20; }
            set { Side = 20; }
        }

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
        /// <param name="propety"></param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propety)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(NameOf.Property(propety)));
            }
        }

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
