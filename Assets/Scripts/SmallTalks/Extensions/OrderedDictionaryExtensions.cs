using System.Collections.Specialized;

namespace SmallTalks.Extensions
{
    public static class OrderedDictionaryExtensions
    {
        public static void MoveItemToIndex(this OrderedDictionary orderedDictionary, object key, int index)
        {
            var value = orderedDictionary[key];
            orderedDictionary.Remove(key);
            orderedDictionary.Insert(index, key, value);
        }
    }
}