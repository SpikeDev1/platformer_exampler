namespace SpawnSystem
{
    using UnityEngine;

    public class SpawnArea
    {
        private Vector3 axis;
        private float radius;

        public SpawnArea(Vector3 axis, float radius)
        {
            this.axis = axis.normalized;
            this.radius = radius;
        }

        public Vector3 GetPoint()
        {
            var result = new Vector3(Random.Range(-radius, radius) * axis.x, Random.Range(-radius, radius) * axis.y, Random.Range(-radius, radius) * axis.z);
            return result;
        }


        public Vector3[] GetPoints(int count)
        {
            var results = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                results[i] = new Vector3(Random.Range(-radius, radius) * axis.x, Random.Range(-radius, radius) * axis.y, Random.Range(-radius, radius) * axis.z);
                
            }
            return results;
        }
    }
}

