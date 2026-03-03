using System.Collections.Generic;
using System.Linq;

namespace SmallTalks.Extensions
{
    public static class StackExtensions
    {
        public static T PopOrDefault<T>(this Stack<T> stack)
        {
            return stack.Any() ? stack.Pop() : default;
        }

        public static T PeekOrDefault<T>(this Stack<T> stack)
        {
            return stack.Any() ? stack.Peek() : default;
        }
    }
}