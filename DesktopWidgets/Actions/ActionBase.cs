using System;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.Classes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Actions
{
    public abstract class ActionBase
    {
        [PropertyOrder(0)]
        [DisplayName("Delay")]
        public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(0);

        [PropertyOrder(1)]
        [DisplayName("Show Errors")]
        public bool ShowErrors { get; set; } = false;

        public void Execute()
        {
            DelayedAction.RunAction((int) Delay.TotalMilliseconds, () =>
            {
                try
                {
                    ExecuteAction();
                }
                catch (Exception ex)
                {
                    if (ShowErrors)
                        Popup.ShowAsync($"{GetType().Name} failed to execute.\n\n{ex.Message}",
                            image: MessageBoxImage.Error);
                }
            });
        }

        protected virtual void ExecuteAction()
        {
        }
    }
}