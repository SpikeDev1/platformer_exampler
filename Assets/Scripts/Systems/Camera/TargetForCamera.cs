using UnityEngine;

namespace Systems.Camera
{
    public class TargetForCamera : MonoBehaviour, ITargetCamera
    {
        [SerializeField] private Transform followTarget;

        public Transform Target => followTarget ? followTarget : transform;
    }
}