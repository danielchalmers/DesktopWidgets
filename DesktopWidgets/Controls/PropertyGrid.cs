namespace DesktopWidgets.Controls
{
    public class PropertyGrid : Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid
    {
        protected override void OnSelectedObjectChanged(object oldValue, object newValue)
        {
            base.OnSelectedObjectChanged(oldValue, newValue);
            ExpandAllProperties();
        }
    }
}