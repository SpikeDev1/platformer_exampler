using System;
using UnityEngine;

namespace Gamemaker.EntitySystem
{
    public class TransformData : ComponentEntity<TransformData>
    {
        public Transform transform;
        public Func<Vector3, Vector3, Vector3> filtersPos;
        public Func<Vector3, Vector3, Vector3> filtersRot;

        public TransformData(Transform transform)
        {
            this.transform = transform;
        }

        public void NewPositionWithFilter(Vector3 v)
        {
            if (filtersPos == null)
            {
                transform.position = v;
            }
            else
            {
                transform.position = filtersPos.Invoke(transform.position, v);
            }
        }

        public void NewRotateWithFilter(Vector3 r)
        {
            if (filtersRot == null)
            {
                transform.rotation = Quaternion.LookRotation(r);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(filtersRot.Invoke(transform.forward, r));
            }
        }
    }
}