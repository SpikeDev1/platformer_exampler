namespace Gamemaker.Misc
{
    using System;
    using System.Collections;
    using TaskManager;
    using UniRx;
    using UnityEngine;


    public class Request<T>
    {
        private Action<T> onReturn;
        private Action<string> onError;
        private Func<Action<T>, Action<string>, IEnumerator> getTask;
        private Task request;

        public Request(Func<Action<T>,Action<string>, IEnumerator> getTask)
        {
            this.getTask = getTask;
        }

        public Request<T> OnOk(Action<T> callback)
        {
            onReturn += callback;
            return this;
        }

        public Request<T> OnError(Action<string> callback)
        {
            onError += callback;
            return this;
        }

        public Request<T> OnError(Action<string> callback, bool repeat, float intervalRepeatInSec = 1f)
        {
            onError = null;
            onError += callback;

            return this;
        }

        public Task Run()
        {
            return new Task(this.getTask(this.onReturn, this.onError));
        }
    }
}