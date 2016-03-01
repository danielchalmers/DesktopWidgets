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

        public virtual void ExecuteAction()
        {
        }
    }
}