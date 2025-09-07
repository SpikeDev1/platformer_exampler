using Core.Utilities;
using Model.Configs;
using UnityEngine;

namespace Gameplay.Spawning
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        public EnemyConfigSO config;
        public PatrolPath path;
    }
}