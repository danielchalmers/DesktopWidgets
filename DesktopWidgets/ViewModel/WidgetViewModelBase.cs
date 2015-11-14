using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DesktopWidgets.Commands;

namespace DesktopWidgets.ViewModel
{
    public abstract class WidgetViewModelBase : INotifyPropertyChanged
    {
        private readonly DispatcherTimer OnTopForceTimer;
        private readonly WidgetSettings Settings;

        private bool _onTop;

        protected WidgetViewModelBase(Guid guid)
        {
            MouseDownCommand = new DelegateCommand(MouseDown);
            Settings = WidgetHelper.GetWidgetSettingsFromGuid(guid);
            if (Settings.ForceOnTop)
            {
                OnTopForceTimer = new DispatcherTimer();
                OnTopForceTimer.Interval = TimeSpan.FromMilliseconds(100);
                OnTopForceTimer.Tick += delegate
                {
                    OnTop = false;
                    OnTop = true;
                };
                OnTopForceTimer.Start();
            }
        }

        public bool OnTop
        {
            get { return _onTop; }
            set
            {
                if (_onTop != value)
                {
                    _onTop = value;
                    RaisePropertyChanged(nameof(OnTop));
                }
            }
        }

        public ICommand MouseDownCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void MouseDown(object parameter)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                (parameter as Window).DragMove();
        }
    }
}