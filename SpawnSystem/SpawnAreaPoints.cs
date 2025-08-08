namespace SpawnSystem
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SpawnAreaPoints
    {
        private List<SpawnAreaCircle> spawnPoints = new List<SpawnAreaCircle>();
        private SpawnAreaCircle.SpawnAreaCircleData data;

        public SpawnAreaPoints(SpawnAreaCircle.SpawnAreaCircleData data, List<Transform> rootAreas)
        {
            this.data = data;
        }

        public int Count
        {
            get
            {
                return spawnPoints.Count;
            }
        }

        public void RefreshAreas(SpawnAreaCircle.SpawnAreaCircleData data, List<Transform> rootAreas)
        {
            this.spawnPoints.Clear();

            var areas = rootAreas.ConvertAll(x => x.gameObject.GetComponent<SpawnAreaCircleSettings>());

            foreach (var point in areas)
            {
                this.spawnPoints.Add(new SpawnAreaCircle(point.GetInstanceID(), data, point.gameObject.GetComponent<Transform>()));
            }
        }

        public void Add(int id, Transform transform)
        {
            spawnPoints.Add(new SpawnAreaCircle(id, this.data, transform.GetComponentInChildren<SpawnAreaCircleSettings>().transform));
        }

        public void Remove(int id)
        {
            spawnPoints.RemoveAll(x => x.ID == id);
        }

        public void Clear()
        {
            spawnPoints.Clear();
        }

        public Vector3 GetRandomPoint()
        {
            if (this.spawnPoints.Count == 0)
            {
                return Vector3.zero;
            }
            var area = this.spawnPoints[Random.Range(0, this.spawnPoints.Count - 1)];
            return area.GetPoint();
        }

        public SpawnAreaCircle GetRandomArea()
        {
            if (this.spawnPoints.Count == 0)
            {
                return null;
            }
            var area = this.spawnPoints[Random.Range(0, this.spawnPoints.Count - 1)];
            return area;
        }

        private SpawnAreaCircle lastSpawnArea;

        public SpawnAreaCircle GetRandomArea(bool unique)
        {
            if (this.spawnPoints.Count == 0)
            {
                return null;
            }
            var area = this.spawnPoints[Random.Range(0, this.spawnPoints.Count - 1)];

            if (lastSpawnArea == area)
            {
                for (int i = 0; i < 2; i++)
                {
                    area = this.spawnPoints[Random.Range(0, this.spawnPoints.Count - 1)];
                    if (lastSpawnArea != area) break;
                }
            }

            lastSpawnArea = area;

            return area;
        }
    }
}