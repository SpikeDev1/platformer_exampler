using System;

namespace Gamemaker.EntitySystem
{
    public static class ArrayExtensions
    {
        public static int Push<T>(this T[] source, T value)
        {
            var index = Array.IndexOf(source, default(T));

            if (index != -1)
            {
                source[index] = value;
            }
            else
            {
                Array.Resize(ref source, source.Length + 1);
                source[source.GetUpperBound(0)] = value;
            }

            return index;
        }
    }
}