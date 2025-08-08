namespace Gamemaker.Misc
{
    using System;
    using System.Collections.Generic;

    public class ObjectCollect
    {
        private static List<object> collection = new List<object>();

        public static event Action<object> onRegistry;
        public static event Action<object> onUnregistry;

        public static void Registry(object obj)
        {
            collection.Add(obj);
            onRegistry?.Invoke(obj);
        }

        public static void Unregistry(object obj)
        {
            collection.Remove(obj);
            onUnregistry?.Invoke(obj);
        }

        public static object Get(Predicate<object> filter)
        {
            return collection.Find(filter);
        }

        public static List<object> Gets(Predicate<object> filter)
        {
            return collection.FindAll(filter);
        }

        public static void Clear()
        {
            collection.Clear();
            onRegistry = null;
            onUnregistry = null;
        }
    }
}