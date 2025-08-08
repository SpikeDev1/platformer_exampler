using UnityEngine;

namespace Gamemaker.Patterns.Pool
{
    using System;

    public interface IPoolObject
    {
        event Action onRelease;
        [Obsolete]
        void SetParent(Transform parent);
    }
}