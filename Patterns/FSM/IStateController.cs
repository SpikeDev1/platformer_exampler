
namespace Gamemaker.Patterns.FSM
{
    public interface IStateController
    {
        IState State { get; }
        void AddState(IState state);
        void SetState<T>() where T : class, IState;
        void SetUpState<T>() where T : class, IState;
        void SetDownState<T>() where T : class, IState;
        void RunCommand(byte command);
        void Update();
    }
}