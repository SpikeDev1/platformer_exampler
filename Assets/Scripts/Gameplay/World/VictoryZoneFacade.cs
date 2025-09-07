using Systems.Combat;
using Model.Signals;
using UnityEngine;
using Zenject;

namespace View.World
{
    [RequireComponent(typeof(Collider2D))]
    public class VictoryZoneFacade : MonoBehaviour
    {
        private SignalBus bus;
        private bool fired;

        [Inject]
        private void Construct(SignalBus bus)
        {
            this.bus = bus;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (fired) return;
            var info = other.GetComponent<ICombatInfo>();
            if (info == null || info.Team != CombatTeam.Player) return;
            fired = true;
            bus.Fire(new PlayerReachedEndLevel());
        }
    }
}