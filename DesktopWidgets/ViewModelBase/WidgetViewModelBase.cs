using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Commands;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.ViewModelBase
{
    public abstract class WidgetViewModelBase : ViewModelBase
    {
        private readonly DispatcherTimer OnTopForceTimer;
        private readonly WidgetSettings Settings;

        private bool _onTop;

        protected WidgetViewModelBase(Guid guid)
        {
            MouseDownCommand = new DelegateCommand(MouseDown);
            LocationChangedCommand = new DelegateCommand(LocationChanged);
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
        public ICommand LocationChangedCommand { get; private set; }

        private void MouseDown(object parameter)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                (parameter as Window).DragMove();
        }

        private void LocationChanged(object parameter)
        {
            if (Settings.SnapToScreenEdges)
                (parameter as Window).SnapToScreenEdges();
        }
    }
}