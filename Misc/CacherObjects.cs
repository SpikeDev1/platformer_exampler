namespace Gamemaker.Misc
{
    using System;
    using System.Collections.Generic;
    using Gamemaker.Patterns.Pool;

    public class CacherObjects<T> : ObjectPool<T> where T :  class, IPoolObject
    {
        private List<T> collection = new List<T>();
        public event Action<T> onRegistry;
        public event Action<T> onUnregistry;

        public CacherObjects(Func<T> crator) : base(crator)
        {
        }

        public CacherObjects(Func<T> creator, int maxInstances, int defaultInstance = 0): base(creator, maxInstances, defaultInstance) {}


        public override T GetObject()
        {
            var obj = base.GetObject();

            this.collection.Add(obj);
            this.onRegistry?.Invoke(obj);

            return obj;
        }

        public override void Release(T obj)
        {
            this.collection.Remove(obj);
            this.onUnregistry?.Invoke(obj);

            base.Release(obj);
        }

        public T Get(Predicate<T> filter)
        {
            return this.collection.Find(filter);
        }

        public List<T> Gets(Predicate<T> filter)
        {
            return this.collection.FindAll(filter);
        }

        public List<T> All()
        {
            return this.collection;
        }

        public override void Clear(Action<T> onPreRemove)
        {
            this.collection.ForEach(x =>
                {
                    onPreRemove?.Invoke(x);
                });

            this.collection.Clear();

            base.Clear(onPreRemove);
        }

        public void Clear()
        {
            this.collection.Clear();
            this.onRegistry = null;
            this.onUnregistry = null;
        }
    }
}