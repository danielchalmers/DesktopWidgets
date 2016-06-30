using System.Collections.Generic;

namespace DesktopWidgets.Helpers
{
    public static class ListHelper
    {
        private static T Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            var tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list[indexB];
        }

        public static T MoveUp<T>(this IList<T> list, T item, bool toEnd = false)
        {
            var index = list.IndexOf(item);
            if (toEnd)
                return list.Swap(index, 0);
            if (index == 0)
                return list.MoveDown(item, true);
            return list.Swap(index, index - 1);
        }

        public static T MoveDown<T>(this IList<T> list, T item, bool toEnd = false)
        {
            var index = list.IndexOf(item);
            if (toEnd)
                return list.Swap(index, list.Count - 1);
            if (list.Count - 1 < index + 1)
                return list.MoveUp(item, true);
            return list.Swap(index, index + 1);
        }
    }
}