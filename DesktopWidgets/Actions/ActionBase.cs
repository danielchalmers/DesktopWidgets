using System;
using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Actions
{
    public abstract class ActionBase
    {
        [DisplayName("Delay")]
        public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(0);

        public void Execute()
        {
            DelayedAction.RunAction((int) Delay.TotalMilliseconds, ExecuteAction);
        }

        public virtual void ExecuteAction()
        {
        }
    }
}