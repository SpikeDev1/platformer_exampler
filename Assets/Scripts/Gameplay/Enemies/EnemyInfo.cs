using Systems.Combat;
using Systems.Movement;
using Model.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyInfo : MonoBehaviour, ICombatInfo
    {
        private DamageConfig attack;
        private IRigidbody2DAdapter body;

        [field: SerializeField] public CombatTeam Team { get; } = CombatTeam.Enemy;

        public Transform Transform => transform;
        public bool IsGrounded => body != null && body.IsGrounded;
        public int ContactDamage => attack.damage;

        [Inject]
        private void Construct(IRigidbody2DAdapter body, DamageConfig attack)
        {
            this.body = body;
            this.attack = attack;
        }
    }
}