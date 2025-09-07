using System;
using UnityEngine;
using Zenject;

namespace Model.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player")]
    public class PlayerConfigSO : ScriptableObjectInstaller<PlayerConfigSO>
    {
        public DamageConfig attack;
        public HealthConfig health;
        public MoveConfig move;
        public GameObject prefab;

        public override void InstallBindings()
        {
            Container.BindInstance(this);
            Container.BindInstance(move).WithId("Player");
            Container.BindInstance(health).WithId("Player");
            Container.BindInstance(attack).WithId("Player");
        }
    }

    [Serializable]
    public class HealthConfig
    {
        public int amount = 1;
        public string deathAudio;
        public string ouchAudio;
    }

    [Serializable]
    public class DamageConfig
    {
        public int damage = 1;
    }

    [Serializable]
    public class MoveConfig
    {
        public string jumpAudio;
        public float jumpTakeOffSpeed = 7f;
        public float maxSpeed = 7f;

        public float sensitivityOffset = 0.08f;
        public float smooth = 12f;
    }

    [Serializable]
    public class PatrolConfig
    {
        public float maxGroundSpeedForJump = 0.02f;
        public float minMoveIntent = 0.01f;
        public bool requireGroundedForJump = true;
        public float stopDistance = 0.05f;
        public float turnDistance = 0.15f;
    }
}