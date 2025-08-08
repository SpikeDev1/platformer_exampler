namespace SpawnSystem
{
    using UnityEngine;

    public class SpawnAreaCircle
    {
        [System.Serializable]
        public class SpawnAreaCircleData
        {
            public Vector3 axisScale;
            public float radius;
        }

        public int ID { get; set; }

        public SpawnAreaCircleData data;
        private Transform center;
        private int numPoint;

        public SpawnAreaCircle(int id, SpawnAreaCircleData data, Transform center)
        {
            ID = id;
            this.data = data;
            this.center = center;
        }

        public Vector3 GetPoint()
        {
            Vector3 result;
            result = GetLocalPoint();
            result += this.center.position;
            return result;
        }

        public Vector3 GetLocalPoint()
        {
            var radius = this.data.radius;
            Vector3 result;

            if (numPoint == 4)
            {
                this.numPoint = 0;
            }

            switch (numPoint)
            {
                case 0:
                    {
                        result = new Vector3(-radius * this.data.axisScale.x, this.data.axisScale.y, 0f); // Random.Range(-radius, radius) *
                        break;
                    }

                case 1:
                    {
                        result = new Vector3(radius * this.data.axisScale.x, this.data.axisScale.y, 0f);
                        break;
                    }

                case 2:
                    {
                        result = new Vector3(0f, this.data.axisScale.y, -radius * this.data.axisScale.z);
                        break;
                    }

                case 3:
                    {
                        result = new Vector3(0f, this.data.axisScale.y, radius * this.data.axisScale.z);
                        break;
                    }

                default: result = new Vector3(0f, this.data.axisScale.y, radius * this.data.axisScale.z); break;
            }

            numPoint++;
            return result;
        }

        public Vector3 GetPointRandom()
        {
            var radius = this.data.radius;
            Vector3 results = new Vector3(Random.Range(-radius, radius) * this.data.axisScale.x, Random.Range(-radius, radius) * this.data.axisScale.y, Random.Range(-radius, radius) * this.data.axisScale.z);
            results += this.center.position;
            return results;
        }

        public Vector3[] GetPoints(int count)
        {
            var radius = this.data.radius;
            var results = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                results[i] = new Vector3(Random.Range(-radius, radius) * this.data.axisScale.x, Random.Range(-radius, radius) * this.data.axisScale.y, Random.Range(-radius, radius) * this.data.axisScale.z);
                results[i] += this.center.position;

            }
            return results;
        }



        public Vector3[] GetLocalPoints(int count)
        {
            var radius = this.data.radius;
            var results = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                results[i] = new Vector3(Random.Range(-radius, radius) * this.data.axisScale.x, Random.Range(-radius, radius) * this.data.axisScale.y, Random.Range(-radius, radius) * this.data.axisScale.z);
            }
            return results;
        }
    }
}