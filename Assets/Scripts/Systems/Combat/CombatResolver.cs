using View.World;

namespace Systems.Combat
{
    public class CombatResolver : ICombatResolver
    {
        private const float eps = 0.2f;

        public virtual void Resolve(ICombatInfo a, IDamageable aDmg, ICombatInfo b, IDamageable bDmg)
        {
            if (a == null || b == null) return;

            if (TouchDeathZone(a, aDmg, b, bDmg)) return;

            if (aDmg == null && bDmg == null) return;

            JumpOn(a, aDmg, b, bDmg);
        }

        private bool TouchDeathZone(ICombatInfo a, IDamageable aDmg, ICombatInfo b, IDamageable bDmg)
        {
            if (a is DeathZoneFacade && bDmg != null)
            {
                bDmg.Kill();
                return true;
            }

            if (b is DeathZoneFacade && aDmg != null)
            {
                aDmg.Kill();
                return true;
            }

            return false;
        }

        private bool JumpOn(ICombatInfo a, IDamageable aDmg, ICombatInfo b, IDamageable bDmg)
        {
            var ay = a.Transform.position.y;
            var by = b.Transform.position.y;

            if (bDmg.IsAlive && aDmg.IsAlive)
            {
                if (ay > by + eps)
                {
                    bDmg.Damage(a.ContactDamage);
                }
                else
                {
                    aDmg.Damage(b.ContactDamage);
                }

                return true;
            }

            return false;
        }
    }
}