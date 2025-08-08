using System;
using System.Collections;

namespace Gamemaker.Patterns.Pool
{
    /// <summary>
    /// Implementation of an object pool using strong references
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : class, IPoolObject
    {
        /// <summary>
        /// Collection containing managed objects
        /// </summary>
        private ArrayList pool;

        /// <summary>
        /// Reference to the object responsible for creating pool objects
        /// </summary>
        private Func<T> creator;

        /// <summary>
        /// Number of objects currently existing
        /// </summary>
        private int instanceCount;

        /// <summary>
        /// Maximum number of objects managed by the pool
        /// </summary>
        private int maxInstances;

        /// <summary>
        /// Creates an object pool
        /// </summary>
        /// <param name="creator">The object responsible for creating managed instances</param>
        public ObjectPool(Func<T> creator)
            : this(creator, 20)
        {
        }

        /// <summary>
        /// Creates an object pool
        /// </summary>
        /// <param name="creator">The object responsible for creating managed instances</param>
        /// <param name="maxInstances">The maximum number of instances the pool allows to exist simultaneously</param>
        public ObjectPool(Func<T> creator, int maxInstances, int defaultInstance = 0)
        {
            this.creator = creator;
            this.instanceCount = 0;
            this.maxInstances = maxInstances;
            this.pool = new ArrayList();
            //this.semaphore = new Semaphore(0, this.maxInstances);
            for (int i = 0; i < defaultInstance; i++)
                Release(CreateObject());
        }

        /// <summary>
        /// Returns the number of objects in the pool waiting for reuse.
        /// The actual number may be less than this value because it represents 
        /// the number of soft references in the pool.
        /// </summary>
        public int Size
        {
            get
            {
                lock (pool)
                {
                    return pool.Count;
                }
            }
        }

        /// <summary>
        /// Returns the number of objects currently managed by the pool
        /// </summary>
        public int InstanceCount { get { return instanceCount; } }

        /// <summary>
        /// Gets or sets the maximum number of objects the pool allows to exist simultaneously
        /// </summary>
        public int MaxInstances
        {
            get { return maxInstances; }
            set { maxInstances = value; }
        }

        /// <summary>
        /// Retrieves an object from the pool. If the pool is empty, a new object will be created
        /// if the number of managed objects is less than <see cref="ObjectPool{T}.MaxInstances"/>.
        /// Otherwise, returns null.
        /// </summary>
        /// <returns></returns>
        public virtual T GetObject()
        {
            //lock (pool)
            {
                T thisObject = RemoveObject();
                if (thisObject != null)
                    return thisObject;

                if (InstanceCount < MaxInstances)
                    return CreateObject();

                return null;
            }
        }

        /// <summary>
        /// Retrieves an object from the pool. If the pool is empty, a new object will be created
        /// if the number of managed objects is less than <see cref="ObjectPool{T}.MaxInstances"/>.
        /// Otherwise, this method will wait until an object becomes available for reuse.
        /// </summary>
        /// <returns></returns>
        public virtual T WaitForObject()
        {
            lock (pool)
            {
                T thisObject = RemoveObject();
                if (thisObject != null)
                    return thisObject;

                if (InstanceCount < MaxInstances)
                    return CreateObject();
            }
            return WaitForObject();
        }

        public virtual void Clear(Action<T> onPreRemove)
        {
            foreach (var p in pool)
            {
                onPreRemove?.Invoke((T)p);
            }

            this.pool.Clear();
        }

        /// <summary>
        /// Removes and returns an object from the pool
        /// </summary>
        /// <returns></returns>
        private T RemoveObject()
        {
            while (pool.Count > 0)
            {
                var refThis = pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);
                var thisObject = (T)refThis;
                if (thisObject != null)
                    return thisObject;
                instanceCount--;
            }
            return null;
        }

        /// <summary>
        /// Creates a new object managed by this pool
        /// </summary>
        /// <returns></returns>
        private T CreateObject()
        {
            T newObject = creator?.Invoke();
            newObject.onRelease += () => Release(newObject);
            instanceCount++;
            return newObject;
        }

        /// <summary>
        /// Releases an object and puts it back into the pool for reuse
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NullReferenceException"></exception>
        public virtual void Release(T obj)
        {
            if (obj == null)
                throw new NullReferenceException();
            //lock (pool)
            {
                var refThis = obj;
                pool.Add(refThis);
            }
        }
    }
}
