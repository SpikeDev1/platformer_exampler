using Model.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerFactory
    {
        private readonly PlayerConfigSO cfg;
        private readonly DiContainer container;

        public PlayerFactory(PlayerConfigSO cfg, DiContainer container)
        {
            this.cfg = cfg;
            this.container = container;
        }

        public PlayerInfo Create(Vector3 position)
        {
            var go = container.InstantiatePrefab(cfg.prefab, position, Quaternion.identity, null);
            return go.GetComponent<PlayerInfo>();
        }
    }
}