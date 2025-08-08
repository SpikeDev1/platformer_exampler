using System.Collections.Generic;
using UniRx;

namespace Gamemaker.Misc
{
    using System;
    using UnityEngine;

    public class ReceiveImpact : MonoBehaviour
    {
        public class ImpactData
        {
            public int idEntityDamager;
            public float mass;
            public float speed;
            public Vector3 direction;
            public string materialObject1;
            public string materialObject2;
            public Vector3 pointWorldCollision;
            public Vector3 normalWorldCollision;
        }

        public Action<ImpactData> onImpact;

        private MaterialObject materalObject2;

        public void Start()
        {
            materalObject2 = gameObject.GetComponent<MaterialObject>();
            onImpact += x =>
                {
                    x.materialObject2 = this.materalObject2.nameSurface;
                };
        }
    }


}