using Systems.Combat;
using UnityEngine;

namespace View.World
{
    [RequireComponent(typeof(Collider2D))]
    public class DeathZoneFacade : MonoBehaviour, ICombatInfo
    {
        public bool grounded = true;

        public CombatTeam Team => CombatTeam.Neutral;
        public Transform Transform => transform;
        public bool IsGrounded => grounded;
        public int ContactDamage => int.MaxValue;
    }
}