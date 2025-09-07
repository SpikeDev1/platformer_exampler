namespace Systems.Combat
{
    public interface ICombatResolver
    {
        void Resolve(ICombatInfo a, IDamageable aDmg, ICombatInfo b, IDamageable bDmg);
    }
}