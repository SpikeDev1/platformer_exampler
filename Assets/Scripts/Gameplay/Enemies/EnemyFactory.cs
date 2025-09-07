using Core.Utilities;
using Model.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyFactory
    {
        private readonly DiContainer container;
        private readonly EnemyConfigSO enemyCfg;

        public EnemyFactory(DiContainer container, EnemyConfigSO enemyCfg)
        {
            this.container = container;
            this.enemyCfg = enemyCfg;
        }

        public GameObject Create(EnemyConfigSO cfg, Vector3 position, PatrolPath path)
        {
            var go = container.InstantiatePrefab(cfg.prefab, position, Quaternion.identity, null);
            var prov = go.GetComponent<PatrolPathAdapter>();

            if (prov) prov.path = path;
            return go;
        }
    }
}