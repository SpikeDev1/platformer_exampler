namespace SpawnSystem
{
    using UnityEngine;

    public class SpawnAreaCircleSettings : MonoBehaviour
    {
        public SpawnAreaCircle.SpawnAreaCircleData data;
        public bool overrideGlobalData = true;

        public void OnDrawGizmosSelected()
        {
            if (this.overrideGlobalData)
            {
                Gizmos.DrawLine(this.GetComponent<Transform>().position + Vector3.left * this.data.radius * this.data.axisScale.x, this.GetComponent<Transform>().position + Vector3.right * this.data.radius * this.data.axisScale.x);
                Gizmos.DrawLine(this.GetComponent<Transform>().position + Vector3.forward * this.data.radius * this.data.axisScale.z, this.GetComponent<Transform>().position + Vector3.back * this.data.radius * this.data.axisScale.z);
            }
        }
    }
}