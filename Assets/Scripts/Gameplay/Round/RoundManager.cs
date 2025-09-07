using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Gameplay.Enemies;
using Gameplay.Players;
using Gameplay.Players.FSM;
using Gameplay.Spawning;
using Model.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace Gameplay.Round
{
    public class RoundManager : IInitializable, IDisposable
    {
        private readonly SignalBus bus;
        private readonly EnemyFactory enemyFactory;
        private readonly PlayerFactory playerFactory;
        private readonly RoundInfo round;
        private readonly RoundSetup setup;

        public RoundManager(PlayerFactory playerFactory, EnemyFactory enemyFactory, SignalBus bus, RoundInfo round,
            [InjectOptional] RoundSetup setup)
        {
            this.playerFactory = playerFactory;
            this.enemyFactory = enemyFactory;
            this.bus = bus;
            this.round = round;
            this.setup = setup;
        }

        public void Dispose()
        {
            bus.Unsubscribe<PlayerReachedEndLevel>(OnVictory);
        }

        public void Initialize()
        {
            bus.Subscribe<PlayerReachedEndLevel>(OnVictory);
            Run().Forget();
        }

        private async UniTaskVoid Run()
        {
            round.InitFor(SceneManager.GetActiveScene().name);

            var spawnPos = Vector3.zero;
            var playerSpawn = setup && setup.playerSpawnPoint ? setup.playerSpawnPoint : null;
            if (playerSpawn) spawnPos = playerSpawn.transform.position;
            var player = playerFactory.Create(spawnPos);

            var enemySpawns = setup && setup.enemySpawnPoints != null && setup.enemySpawnPoints.Length > 0
                ? setup.enemySpawnPoints
                : null;
            if (enemySpawns != null)
            {
                for (var i = 0; i < enemySpawns.Length; i++)
                {
                    var s = enemySpawns[i];
                    if (s && s.config) enemyFactory.Create(s.config, s.transform.position, s.path);
                }
            }

            var vcam = Object.FindFirstObjectByType<CinemachineVirtualCamera>();
            if (vcam) vcam.Follow = player.Target;

            await UniTask.Yield();
        }

        private void OnVictory()
        {
            round.Complete();
        }

        private void NewRound(IPlayerSpawnerView vew, IPlayerStateInfo playerState)
        {
            // get spawn point 
            var pos = Vector3.zero;
            var playerSpawn = setup && setup.playerSpawnPoint
                ? setup.playerSpawnPoint
                : Object.FindFirstObjectByType<PlayerSpawnPoint>();
            if (playerSpawn) pos = playerSpawn.transform.position;

            // respawn character
            vew.Respawn(pos);
            playerState.Id = PlayerStateId.Spawn;
        }

        public void OnPlayerDied(PlayerDiedSignal s)
        {
            NewRound(s.Spawner, s.PlayerStateInfo);
        }
    }
}