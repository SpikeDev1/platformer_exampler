using System;
using System.Collections.Generic;

namespace Gamemaker.Patterns.Publisher
{
    public delegate void ReturnAdress(params object[] args);

    public class Publisher<T> : IProgress, IPublisher<T> where T: struct , IConvertible
    {
        private Dictionary<T,ReturnAdress> adressBook = new Dictionary<T,ReturnAdress>();

        public virtual void Subscribe(T channel, ReturnAdress observer)
        {
            if (!adressBook.ContainsKey(channel))
                adressBook.Add(channel, observer);
            else adressBook[channel] += observer;
        }

        public virtual void Unsubscribe(T channel, ReturnAdress observer)
        {
            if (!adressBook.ContainsKey(channel)) return;
            adressBook[channel] -= observer;
        }

        public virtual void Publish(T channel, params object[] args)
        {
            progress = channel;
            if (!adressBook.ContainsKey(channel)) return;
            ReturnAdress observer = adressBook[channel];
            observer(args);
        }

        public void Clear()
        {
            adressBook.Clear();
            progress01 = 0f;
            progress = new T();
        }

        private T progress;
        /// <summary>
        /// Get current value type of T
        /// </summary>
        public string Progress
        {
            get { return progress.ToString(); }
        }

        private float progress01;
        public float Progress01
        {
            get { return progress01; }
            set { progress01 = value; }
        }
    }
}