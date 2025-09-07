using Systems.Combat;
using UnityEngine;
using Zenject;

namespace Gameplay.Token
{
    [RequireComponent(typeof(Collider2D))]
    public class TokenZone : MonoBehaviour
    {
        private IPickupModel pickup;

        [Inject]
        private void Construct([InjectOptional] IPickupModel pickup)
        {
            this.pickup = pickup;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var info = other.GetComponentInParent<ICombatInfo>();
            if (info == null || info.Team != CombatTeam.Player) return;

            if (!pickup.Collected.Value)
            {
                pickup.SetCollected(true);
                other.GetComponentInParent<ITokenCollector>()?.CollectToken();
            }
        }
    }
}