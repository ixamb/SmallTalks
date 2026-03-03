using System.Collections.Generic;

namespace SmallTalks.Extensions
{
    public static class ListExtensions
    {
        public static void AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
                list.Add(item);
        }

        public static void RemoveUnique<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
                list.Remove(item);
        }

        public static bool IsIndexValid<T>(this List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }
        
        public static void MoveItemToIndex<T>(this List<T> list, T item, int index)
        {
            list.Remove(item);
            list.Insert(index, item);
        }
    }
}