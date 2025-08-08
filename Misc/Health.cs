

using Gamemaker.EntitySystem;

namespace Gamemaker.Misc
{
    using System;
    using UniRx;

    public class Health : ComponentEntity<Health>, IDisposable
    {
        [System.Serializable]
        public class HealthData
        {
            public float startAmount;
            public float maxAmount;
            public float minAmount;
        }

        public ReactiveProperty<float> Amount { get; private set; }
        public ReactiveProperty<float> Max { get; private set; }
        public ReactiveProperty<float> Min { get; private set; }

        private HealthData data;

        public float Amount01
        {
            get
            {
                return Amount.Value / Max.Value;
            }
        }

        public void Dispose()
        {
            Amount?.Dispose();
            Max?.Dispose();
            Min?.Dispose();

            Amount = null;
            Max = null;
            Min = null;
            data = null;
        }

        public Health(HealthData data)
        {
            this.data = data;
            Amount = new FloatReactiveProperty(data.startAmount);
            Max = new FloatReactiveProperty(data.maxAmount);
            Min = new FloatReactiveProperty(data.minAmount);
        }

        public void Increase(float amount)
        {
            var newAmount = Amount.Value + amount;

            if (newAmount > Max.Value)
            {
                Amount.SetValueAndForceNotify(Max.Value);
            }
            else
            {
                Amount.SetValueAndForceNotify(newAmount);
            }
        }

        public void Decrease(float amount)
        {
            var newAmount = Amount.Value - amount;

            if (newAmount < Min.Value)
            {
                Amount.SetValueAndForceNotify(Min.Value);
            }
            else
            {
                Amount.SetValueAndForceNotify(newAmount);
            }
        }
    }
}

