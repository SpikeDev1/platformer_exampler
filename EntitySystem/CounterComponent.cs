namespace Gamemaker.EntitySystem
{
    public class CounterComponent : ComponentEntity<CounterComponent>
    {
        [System.Serializable]
        public struct Data
        {
            public float startAmount;
            public float maxAmount;
            public float minAmount;
        }

        [System.Serializable]
        public struct State
        {
            public float currentAmount;
            public float maxAmount;
            public float minAmount;
        }

        public Data data;
        public State state;

        public CounterComponent(Data data)
        {
            this.data = data;
            this.state = new State() { currentAmount = data.startAmount, minAmount = data.minAmount, maxAmount = data.maxAmount };
        }
    }
}