using Systems.Combat;
using Systems.Movement;
using Gameplay.Players.FSM;
using Gameplay.Token;
using Model.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerInfo : MonoBehaviour, ICombatInfo
    {
        private DamageConfig attack;
        private IContactGround body;

        private ITokenCollector tokenCollector;
        private IPlayerDamageFacade _damageView;
        public Transform Target => _damageView.Transform;
        public PlayerStateId PlayerState { get; set; }

        [field: SerializeField] public CombatTeam Team { get; } = CombatTeam.Player;

        public Transform Transform => _damageView.Transform;
        public bool IsGrounded => body != null && body.IsGrounded;
        public int ContactDamage => attack.damage;

        [Inject]
        private void Construct(IContactGround body, DamageConfig attack, ITokenCollector tokenCollector,
            IPlayerDamageFacade damageView, IPlayerStateInfo playerStateInfo)
        {
            this.body = body;
            this.attack = attack;
            this.tokenCollector = tokenCollector;
            this._damageView = damageView;
        }

        public void CollectToken()
        {
            tokenCollector.CollectToken();
        }
    }
}