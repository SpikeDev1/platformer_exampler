using UnityEngine;

namespace Systems.Camera
{
    public interface ITargetCamera
    {
        Transform Target { get; }
    }
}