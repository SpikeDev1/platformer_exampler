
namespace Gamemaker.Patterns.FSM
{
    using System;
    using System.Collections;

    public interface IState
    {
        Action<IState> AddState { get; set; }
        Action<IState> SetState { get; set; }
        void OnEnter();
        void Update();
        void OnLeave();
        IEnumerator OnLeaveAsync();
        void OnLeaveDown();
        void RunCommand(byte command);
        bool IsCommand(byte actionCommand);
    }
}