using UnityEngine;

namespace Systems.Combat
{
    public interface ICombatInfo
    {
        CombatTeam Team { get; }
        Transform Transform { get; }
        bool IsGrounded { get; }
        int ContactDamage { get; }
    }
}