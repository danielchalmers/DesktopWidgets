using System;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for UpdatePrompt.xaml
    /// </summary>
    public partial class UpdateProgress : Window, INotifyPropertyChanged
    {
        private double _currentProgress;

        public UpdateProgress(Version updateVersion)
        {
            InitializeComponent();

            HelpText =
                $"Downloading update ({updateVersion})\n{Properties.Resources.AppName} will restart when complete.";

            DataContext = this;
        }

        public string HelpText { get; set; }

        public double CurrentProgress
        {
            get => _currentProgress;
            set
            {
                if (value.IsEqual(_currentProgress))
                {
                    _currentProgress = value;
                    RaisePropertyChanged(nameof(CurrentProgress));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}