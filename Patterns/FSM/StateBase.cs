namespace Gamemaker.Patterns.FSM
{
    using System;
    using System.Collections;
    using System.Threading;
    using TaskManager;
    using UnityEngine;

    public class StateBase : IState
    {
        private Action ParentAction { get; set; }
        public StateBaseController controller;

        private StateBaseController childController; // maby need impl multy controllers
        private Status status;

        public Status StatusState { get { return this.status; } }

        public Action<IState> AddState { get; set; }
        public Action<IState> SetState { get; set; }

        public enum Status
        {
            Entered,
            Entering,
            Leaved,
            Leaving
        }

        public void ToState<T>(T state) where T : StateBase
        {
            this.AddState(state);
            this.SetState(state);
        }

        public virtual void OnEnter()
        {
            status = Status.Entered;
        }

        public virtual void Update()
        {
            if (this.childController != null)
            {
                this.childController.Update();
            }
        }

        public virtual void OnLeave()
        {
            if (this.status != Status.Leaving)
            {
                this.status = Status.Leaving;

                this.MoveToParent();
            }
        }

        public virtual void OnLeaveDown()
        {
        }

        public virtual void RunCommand(byte command)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsCommand(byte actionCommand)
        {
            throw new NotImplementedException();
        }

        public void RunChildState<T>(T state, Action onLeaveTheState) where T : class, IState
        {
            if (this.childController != null)
            {
                if (((StateBase)this.childController.State).StatusState != Status.Leaving)
                {
                    this.childController.State.OnLeave();
                    ((StateBase)this.childController.State).status = Status.Leaved;
                }
            }

            this.childController = new StateBaseController();
            this.childController.AddState(state);
            this.childController.SetState<T>();
            if (((StateBase)this.childController.State).StatusState == Status.Leaving)
            {
                this.OnStopedChildState();
                if (onLeaveTheState != null)
                {
                    onLeaveTheState();
                }
            }
            else
            {
                ((StateBase)this.childController.State).ParentAction = this.OnStopedChildState;
                ((StateBase)this.childController.State).ParentAction += onLeaveTheState;
            }
        }

        public Task RunChildStateAsync<T>(T state,Action onStartTheState, Action onLeaveTheState) where T : class, IState
        {
            Action<bool> runNextState = x =>
                {
                    this.childController = new StateBaseController();
                    this.childController.AddState(state);
                    this.childController.SetState<T>();

                    if (((StateBase)this.childController.State).StatusState == Status.Leaving)
                    {
                        this.OnStopedChildState();
                        if (onLeaveTheState != null)
                        {
                            onLeaveTheState();
                        }
                    }
                    else
                    {
                        ((StateBase)this.childController.State).ParentAction = this.OnStopedChildState;
                        ((StateBase)this.childController.State).ParentAction += onLeaveTheState;
                        if (onStartTheState != null)
                        {
                            onStartTheState();
                        }
                    }
                };

            if (this.childController != null)
            {
                if (this.childController.State != null && ((StateBase)this.childController.State).StatusState != Status.Leaving)
                {
                    var task = new Task(this.childController.State.OnLeaveAsync());
                    task.Finished += manual =>
                        {
                            runNextState(manual);
                            ((StateBase)this.childController.State).status = Status.Leaved;
                        };
                    return task;

                }
                else
                {
                    runNextState(false);
                }
            }
            else
            {
                    runNextState(false);
            }

            return null;
        }

        public void StopChildState()
        {
            if (this.childController == null)
            {
                return;
            }

            var prevChildState = (StateBase)this.childController.State;
            if (prevChildState.StatusState != Status.Leaving)
            {
                this.childController.State.OnLeave();
            }

            if (this.childController != null && prevChildState == (StateBase)this.childController.State)
            {
                this.OnStopedChildState();
            }
        }

        public Task StopChildStateAsync()
        {
            if (this.childController == null)
            {
                return null;
            }

            var prevChildState = (StateBase)this.childController.State;
            if (prevChildState.StatusState != Status.Leaving)
            {
                var task = new Task(this.childController.State.OnLeaveAsync());
                return task;
            }

            if (this.childController != null && prevChildState == (StateBase)this.childController.State)
            {
                this.OnStopedChildState();
            }

            return null;
        }

        public void StopChildState<T>() where T : class, IState
        {
            if (this.childController == null)
            {
                return;
            }

            if (this.childController.State.GetType() != typeof(T))
            {
                return;
            }

            StateBase prevChildState = (StateBase)this.childController.State;
            if (prevChildState.StatusState != Status.Leaving)
            {
                this.childController.State.OnLeave();

            }

            if (this.childController != null && prevChildState == (StateBase)this.childController.State)
            {
                this.OnStopedChildState();
            }
        }

        public bool IsChildState<T>() where T : class, IState
        {
            if (this.childController == null)
            {
                return false;
            }

            return this.childController.State.GetType() == typeof(T);
        }

        private void OnStopedChildState()
        {
            this.childController = null;
        }

        private void MoveToParent()
        {
            if (this.ParentAction != null)
            {
                this.ParentAction();
            }

            this.ParentAction = null;
        }

        public virtual IEnumerator OnLeaveAsync()
        {

            yield return null;
        }
    }

}