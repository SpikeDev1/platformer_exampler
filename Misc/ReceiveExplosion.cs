namespace Gamemaker.Misc
{
    using System;
    using UnityEngine;

    public class ReceiveExplosion : MonoBehaviour
    {
        public class ExplosionData
        {
            public float power;
            public Vector3 pointWorldSource;
        }

        public Action<ExplosionData> onExplosion;
    }
}