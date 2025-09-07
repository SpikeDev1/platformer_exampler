using UnityEngine;

namespace Core.Utilities
{
    public class PatrolPathAdapter : MonoBehaviour, IPatrolPath
    {
        public PatrolPath path;

        public PatrolPath.Mover CreateMover(float speed)
        {
            if (path == null) return null;

            return path.CreateMover(speed);
        }
    }
}