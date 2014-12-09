namespace HelixPerfBox
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media.Media3D;
    using Annotations;
    using Gu.Reactive;
    using System.Reactive;

    public class ViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Ball> _redBalls = new ObservableCollection<Ball>();
        private readonly ObservableCollection<Ball> _blueBalls = new ObservableCollection<Ball>();

        private Ball _selectedRed;
        private bool _isBallsVisible = true;


        public ViewModel()
        {
            this.ToObservable(x => x.IsBallsVisible)
                .Subscribe(
                    x =>
                        {
                            if (IsBallsVisible)
                            {
                                CreateBalls(10);
                            }
                            else
                            {
                                _blueBalls.Clear();
                                _redBalls.Clear();
                            }
                        });
        }

        private void CreateBalls(int side)
        {
            for (int x = 0; x < side; x++)
            {
                for (int y = 0; y < side; y++)
                {
                    _redBalls.Add(new Ball(new Point3D(x, y, 0), 0.3));
                    _blueBalls.Add(new Ball(new Point3D(x, y, 5), 0.3));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Ball> RedBalls
        {
            get { return _redBalls; }
        }

        public Ball SelectedRed
        {
            get { return _selectedRed; }
            set
            {
                if (Equals(value, _selectedRed))
                {
                    return;
                }
                _selectedRed = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Ball> BlueBalls
        {
            get { return _blueBalls; }
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


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
