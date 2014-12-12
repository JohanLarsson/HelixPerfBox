﻿namespace HelixPerfBox
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SandBox.xaml
    /// </summary>
    public partial class SandBox : UserControl
    {
        public SandBox()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Status.Text = "Clicked: " + sender.GetType().Name;
        }
    }
}
