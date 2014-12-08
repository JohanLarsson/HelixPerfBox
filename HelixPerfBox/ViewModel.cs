namespace HelixPerfBox
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using Annotations;
    using Gu.Reactive;
    using Gu.Wpf.Reactive;

    public class ViewModel : INotifyPropertyChanged
    {
        private Ball _selectedRed;
        private bool _isBallsVisible = true;
        public ViewModel()
        {
            var redBalls = new List<Ball>();
            var blueBalls = new List<Ball>();
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    redBalls.Add(new Ball(new Point3D(x, y, 0), 0.3));
                    blueBalls.Add(new Ball(new Point3D(x, y, 5), 0.3));
                }
            }

            RedBalls = new CollectionView<Ball>(redBalls, this.ToObservable(x => x.IsBallsVisible))
            {
                Filter = _ => IsBallsVisible
            };

            BlueBalls = new CollectionView<Ball>(blueBalls, this.ToObservable(x => x.IsBallsVisible))
            {
                Filter = _ => IsBallsVisible
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CollectionView<Ball> RedBalls { get; private set; }

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

        public CollectionView<Ball> BlueBalls { get; private set; }

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
