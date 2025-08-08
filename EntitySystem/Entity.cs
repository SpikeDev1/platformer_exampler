using System.Collections.Generic;
using System.Linq;
using Gamemaker.Misc;

namespace Gamemaker.EntitySystem
{
    public abstract class Entity
    {
        private List<ComponentEntity> comps = new List<ComponentEntity>();

        public void Destroy()
        {
            foreach (var componentEntity in comps.ToArray())
            {
                componentEntity.Destroy();
            }
            comps.Clear();
        }

        public void AddComponent(ComponentEntity comp)
        {
            comp.SetEntity(this);
            comp.onDestroy = () => RemoveComponent(comp);
            comps.Add(comp);
        }

        public void RemoveComponent(ComponentEntity comp)
        {
            comps.Remove(comp);
        }

        public T FindComp<T>() where T : ComponentEntity
        {
            return comps.FirstOrDefault(x => x is T) as T;
        }

        public List<T> FindCompAll<T>() where T : ComponentEntity
        {
            return comps.FindAll(x => x is T).ConvertAll( x => x as T);
        }

        public virtual bool IsComp<T>() where T : ComponentEntity
        {
            return comps.Exists(x => x is T);
        }

    }
}
