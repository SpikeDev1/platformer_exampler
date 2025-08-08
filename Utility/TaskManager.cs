using System.Collections.Generic;

namespace TaskManager
{
    using System.Collections;
    using UnityEngine;

    public class TaskManager : MonoBehaviour
    {
        public class TaskState
        {
            public bool Running
            {
                get
                {
                    return this.running;
                }
            }

            public bool Paused
            {
                get
                {
                    return this.paused;
                }
            }

            public delegate void FinishedHandler(bool manual);
            public event FinishedHandler Finished;

            IEnumerator coroutine;
            bool running;
            bool paused;
            bool stopped;

            public TaskState(IEnumerator c)
            {
                this.coroutine = c;
            }

            public void Pause()
            {
                this.paused = true;
            }

            public void Unpause()
            {
                this.paused = false;
            }

            public void Start()
            {
                this.running = true;
                singleton.StartCoroutine(this.CallWrapper());
            }

            public void Stop()
            {
                this.stopped = true;
                this.running = false;
            }

            IEnumerator CallWrapper()
            {
                yield return null;
                IEnumerator e = this.coroutine;
                while (this.running)
                {
                    if (this.paused)
                        yield return null;
                    else
                    {
                        if (e != null && e.MoveNext())
                        {
                            yield return e.Current;
                        }
                        else
                        {
                            this.running = false;
                        }
                    }
                }

                this.Finished?.Invoke(this.stopped);
                this.Finished = null;
                this.coroutine = null;
            }
        }

       static TaskManager singleton;

        public static TaskState CreateTask(IEnumerator coroutine)
        {
            if (singleton == null)
            {
                GameObject go = new GameObject("TaskManager");
                if (Application.isPlaying || !Application.isEditor)
                {
                    DontDestroyOnLoad(go);
                }

                singleton = go.AddComponent<TaskManager>();
            }
            return new TaskState(coroutine);
        }

        public static void Destroy()
        {
            if (Application.isEditor)
            {
                GameObject.DestroyImmediate(singleton.gameObject);
            }
            else
            {
                if (singleton)
                    GameObject.Destroy(singleton.gameObject);
            }
        }

        public class TaskQueue
        {
            public Queue<Task> queue = new Queue<Task>();

            public TaskQueue()
            {
            }

            private Task last;

            public void Add(IEnumerator task)
            {
                var taskNew = new Task(task, false);
                if (last != null)
                {
                    last.Finished += manual => taskNew.Start();
                    queue.Enqueue(taskNew);
                }
                else taskNew.Start();

                last = taskNew;
            }

            public void Clear()
            {
                foreach (var q in queue)
                {
                    q.Stop();
                }
            }
        }
    }
}