namespace TaskManager
{
    using System.Collections;
    using UnityEngine;

    /// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
    /// It is an error to attempt to start a task that has been stopped or which has
    /// naturally terminated.
    public class Task
    {

        /// Returns true if and only if the coroutine is running.  Paused tasks
        /// are considered to be running.
        public bool Running
        {
            get
            {
                if (this.task == null)
                {
                    return false;
                }

                return this.task.Running;
            }
        }

        /// Returns true if and only if the coroutine is currently paused.
        public bool Paused
        {
            get
            {
                if (this.task == null)
                {
                    return false;
                }

                return this.task.Paused;
            }
        }

        /// Delegate for termination subscribers.  manual is true if and only if
        /// the coroutine was stopped with an explicit call to Stop().
        public delegate void FinishedHandler(bool manual);

        /// Termination event.  Triggered when the coroutine completes execution.
        public event FinishedHandler Finished;

        /// Creates a new Task object for the given coroutine.
        ///
        /// If autoStart is true (default) the task is automatically started
        /// upon construction.
        public Task(IEnumerator c, bool autoStart = true)
        {
            this.task = TaskManager.CreateTask(c);
            this.task.Finished += this.TaskFinished;
            if (autoStart)
                this.Start();
        }

        public Task(IEnumerator c, FinishedHandler handler, bool autoStart = true)
            : this(c, autoStart)
        {
            this.Finished += handler;
        }

        /// Begins execution of the coroutine
        public void Start()
        {
            this.task?.Start();
        }

        /// Discontinues execution of the coroutine at its next yield.
        public void Stop()
        {
            this.task?.Stop();
            this.task = null;
        }

        public void Pause()
        {
            this.task?.Pause();
        }

        public void Unpause()
        {
            this.task?.Unpause();
        }

        public IEnumerator WaitFinished()
        {
            while (Running)
            {
                yield return null;
            }
        }

        void TaskFinished(bool manual)
        {
            FinishedHandler handler = this.Finished;
            if (handler != null)
                handler(manual);
        }

        TaskManager.TaskState task;

        public class Wait
        {
            public float secWait;
            public float currentTimer;

            public Wait(float secWait)
            {
                this.secWait = secWait;
            }

            public void Reset(float secWait)
            {
                this.secWait = secWait;
                this.currentTimer = 0;
            }

            public bool Waiting()
            {
                if (secWait > currentTimer)
                {
                    currentTimer += Time.deltaTime;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}