using UnityEngine;
using Zenject;

namespace Systems.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class TouchDamageHandler : MonoBehaviour
    {
        private ICombatResolver resolver;

        [Inject]
        public void Construct(ICombatResolver resolver)
        {
            this.resolver = resolver;
        }

        private void OnCollisionEnter2D(Collision2D c)
        {
            Process(c.collider);
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            Process(c);
        }

        private void Process(Collider2D other)
        {
            var aInfo = GetComponentInParent<ICombatInfo>();
            var aDmg = GetComponentInParent<IDamageable>();
            var bInfo = other.GetComponentInParent<ICombatInfo>();
            var bDmg = other.GetComponentInParent<IDamageable>();
            resolver.Resolve(aInfo, aDmg, bInfo, bDmg);
        }
    }
}