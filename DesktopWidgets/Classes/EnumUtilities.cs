namespace DesktopWidgets.Classes
{
    public static class EnumUtilities
    {
        public static bool IsHorizontal(this ScreenDockPosition screenDockPosition)
        {
            return screenDockPosition == ScreenDockPosition.Left || screenDockPosition == ScreenDockPosition.Right;
        }

        public static bool IsVertical(this ScreenDockPosition screenDockPosition)
        {
            return screenDockPosition == ScreenDockPosition.Top || screenDockPosition == ScreenDockPosition.Bottom;
        }
    }
}