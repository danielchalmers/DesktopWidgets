using System;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Actions
{
    public abstract class ActionBase
    {
        [DisplayName("Delay")]
        public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(0);

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
                    Popup.ShowAsync($"{GetType().Name} failed to execute.\n\n{ex.Message}", image: MessageBoxImage.Error);
                }
            });
        }

        public virtual void ExecuteAction()
        {
        }
    }
}