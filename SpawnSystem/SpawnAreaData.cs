namespace SpawnSystem
{
    using UnityEngine;

    public class SpawnAreaData : MonoBehaviour
    {
        public Vector3 axis;
        public float radius;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(this.transform.position, this.radius);
        }
    }
}