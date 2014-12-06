namespace HelixPerfBox
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using Gu.Wpf.Reactive;

    public class ViewModel
    {
        public ViewModel()
        {
            var redBalls = Enumerable.Range(0, 50)
                                     .Select(z => new Ball(new Point3D(0, 0, z), Brushes.Red, 0.3))
                                     .ToArray();
            RedBalls = new CollectionView<Ball>(redBalls);

            var blueBalls = Enumerable.Range(0, 50)
                         .Select(x => new Ball(new Point3D(x, 0, 0), Brushes.Blue, 0.3))
                         .ToArray();
            BlueBalls = new CollectionView<Ball>(blueBalls);
        }

        public CollectionView<Ball> RedBalls { get; private set; }

        public CollectionView<Ball> BlueBalls { get; private set; }
    }
}
