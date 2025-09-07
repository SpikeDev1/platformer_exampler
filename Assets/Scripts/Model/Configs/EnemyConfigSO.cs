using UnityEngine;
using Zenject;

namespace Model.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/Enemy")]
    public class EnemyConfigSO : ScriptableObjectInstaller<GameConfigSO>
    {
        public DamageConfig attack;
        public HealthConfig health;
        public MoveConfig move;
        public PatrolConfig patrol;
        public GameObject prefab;

        public override void InstallBindings()
        {
            Container.BindInstance(this);
            Container.BindInstance(move).WithId("Enemy");
            Container.BindInstance(health).WithId("Enemy");
            Container.BindInstance(attack).WithId("Enemy");
            Container.BindInstance(patrol).WithId("Enemy");
        }
    }
}