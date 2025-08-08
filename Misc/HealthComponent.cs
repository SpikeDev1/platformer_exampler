using Gamemaker.EntitySystem;

namespace Gamemaker.Misc
{
    public class HealthComponent : ComponentEntity<HealthComponent>
    {
        [System.Serializable]
        public struct HealthData
        {
            public float startAmount;
            public float maxAmount;
            public float minAmount;
        }

        [System.Serializable]
        public struct HealthState
        {
            public float amount;
            public float maxAmount;
            public float minAmount;
        }

        public HealthData data;
        public HealthState state;

        public HealthComponent(HealthData data, HealthState state)
        {
            this.data = data;
            this.state = state;
        }
    }
}