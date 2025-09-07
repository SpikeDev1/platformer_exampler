using Gameplay.Spawning;
using Gameplay.Token;
using UnityEngine;

namespace Gameplay.Round
{
    public class RoundSetup : MonoBehaviour, ILevelTokensModel
    {
        public EnemySpawnPoint[] enemySpawnPoints;
        public PlayerSpawnPoint playerSpawnPoint;
        public TokenZone[] tokenZones;

        public int TotalTokens
        {
            get { return tokenZones.Length; }
        }
    }

    public interface ILevelTokensModel
    {
        int TotalTokens { get; }
    }
}