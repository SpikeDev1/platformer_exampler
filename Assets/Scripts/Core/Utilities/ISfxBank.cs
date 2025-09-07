using UnityEngine;

namespace Core.Utilities
{
    public interface ISfxBank
    {
        bool TryGetClip(string id, out AudioClip clip);
    }
}