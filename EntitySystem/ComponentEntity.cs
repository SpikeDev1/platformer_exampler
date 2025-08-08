using System;
using System.Collections;
using TaskManager;
using UniRx;

namespace Gamemaker.EntitySystem
{
    public abstract class ComponentEntity
    {
        public bool IsUpdate { get; protected set; }
        public float wait;
        public Action onDestroy;
        protected Entity entity;

        public ComponentEntity()
        {
            IsUpdate = true;
        }

        public void SetEntity(Entity entity)
        {
            this.entity = entity;
        }

        public virtual void Destroy()
        {
            IsUpdate = false;
            onDestroy?.Invoke();
            onDestroy = null;
            entity = null;
        }

        public virtual bool IComp<T>() 
            where T: ComponentEntity 
        {
            if (entity == null) return false;

            return entity.IsComp<T>();
        }

        public void Run(Action update)
        {
            MainThreadDispatcher.StartUpdateMicroCoroutine(Update(update));
        }

        public void Stop()
        {
            IsUpdate = false;
        }

        private IEnumerator Update(Action update)
        {
            while (true)
            {
                if (!IsUpdate) yield break;

                update?.Invoke();

                if (!IsUpdate) yield break;

                if (wait > 0f)
                {
                    var wait = new Task.Wait(this.wait);
                    while (wait.Waiting())
                    {
                        if (!IsUpdate) yield break;

                        yield return null;
                    }
                }

                yield return null;
            }
        }
    }

    public abstract class ComponentEntity<T> : ComponentEntity where T : ComponentEntity
    {
        public virtual T IsComp<S>()
            where S : ComponentEntity
        {
            if (entity == null) return null;

            return entity.IsComp<S>() ? this as T : null;
        }

        public S FindComp<S>() where S : ComponentEntity
        {
            return entity?.FindComp<S>();
        }

        public T SetEntity(Entity entity)
        {
            this.entity = entity;
            this.entity.AddComponent(this);
            return this as T;
        }

        public void Run(Action<T> update)
        {
            IsUpdate = true;
            if (update != null)
                MainThreadDispatcher.StartUpdateMicroCoroutine(Update(update));
        }

        private IEnumerator Update(Action<T> update)
        {
            while (true)
            {
                if (!IsUpdate) yield break;

                update?.Invoke(this as T);

                if (!IsUpdate) yield break;

                yield return null;
            }
        }
    }
}