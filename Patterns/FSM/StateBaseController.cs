using System.Collections.Generic;

namespace Gamemaker.Patterns.FSM
{
    public class StateBaseController : IStateController
    {
        private IState currentState;
        private List<IState> states = new List<IState>();

        public IState State
        {
            get { return currentState; }
        }

        public void AddState(IState state)
        {
            states.Add(state);
            state.AddState = this.AddState;
            state.SetState = this.SetState;
        }

        public void SetState(IState state)
        {
            var t = state.GetType();
            if (currentState != null)
            {
                if (currentState.GetType() == t) return;
                currentState.OnLeave();
            }

            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetType() != t) continue;
                currentState = states[i];
                break;
            }

            currentState.OnEnter();
        }

        public void SetState<T>() where T : class, IState
        {
            if (currentState != null)
            {
                if (currentState.GetType() == typeof(T)) return;
                currentState.OnLeave();
            }

            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetType() != typeof(T)) continue;
                currentState = states[i];
                break;
            }

            currentState.OnEnter();
        }

        public void SetUpState<T>() where T : class, IState
        {
            if (currentState != null)
            {
                if (currentState.GetType() == typeof(T)) return;
            }

            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetType() != typeof(T)) continue;
                currentState = states[i];
                break;
            }

            if (currentState != null)
                currentState.OnEnter();
        }

        public void SetDownState<T>() where T : class, IState
        {
            if (currentState != null)
            {
                if (currentState.GetType() == typeof(T)) return;
                currentState.OnLeaveDown();
            }

            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetType() != typeof(T)) continue;
                currentState = states[i];
                break;
            }

            if (currentState != null)
                currentState.OnEnter();
        }

        public void RunCommand(byte command)
        {
            currentState.RunCommand(command);
        }

        public void Update()
        {
            currentState.Update();
        }

    }
}
