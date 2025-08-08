namespace Gamemaker.Misc
{
    using System;
    using UniRx;
    using UnityEngine;

    public class Counter : IDisposable
    {
        [System.Serializable]
        public class CounterData
        {
            public float startAmount;
            public float maxAmount;
            public float minAmount;
        }

        public Action<float> onIncrease, onDecrease;

        public ReactiveProperty<float> Amount { get; private set; }
        public ReactiveProperty<float> Max { get; private set; }
        public ReactiveProperty<float> Min { get; private set; }

        private CounterData data;

        public float Amount01
        {
            get
            {
                return this.Amount.Value / this.Max.Value;
            }
        }

        public void Dispose()
        {
            data = null;
            onIncrease = null;
            onDecrease = null;
            Amount = null;
            Max = null;
            Min = null;
        }

        public void Destroy()
        {
            Amount.Dispose();
            Max.Dispose();
            Min.Dispose();
        }

        public Counter(CounterData data)
        {
            this.data = data;
            this.Amount = new FloatReactiveProperty(data.startAmount);
            this.Max = new FloatReactiveProperty(data.maxAmount);
            this.Min = new FloatReactiveProperty(data.minAmount);
        }

        public void Increase(float amount)
        {
            onIncrease?.Invoke(amount);

            var newAmount = this.Amount.Value + amount;

            if (newAmount > this.Max.Value)
            {
                this.Amount.SetValueAndForceNotify(this.Max.Value);
            }
            else
            {
                this.Amount.SetValueAndForceNotify(newAmount);
            }
        }

        public void Decrease(float amount)
        {
            this.onDecrease?.Invoke(amount);

            var newAmount = this.Amount.Value - amount;

            if (newAmount < this.Min.Value)
            {
                this.Amount.SetValueAndForceNotify(this.Min.Value);
            }
            else
            {
                this.Amount.SetValueAndForceNotify(Mathf.Round(newAmount));
            }
        }
    }
}