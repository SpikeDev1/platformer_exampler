using Systems.Input;
using UniRx;
using UnityEngine;

namespace Gameplay.Players
{
    public class PlayerControlService : IMovementControl
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly BoolReactiveProperty enabled = new BoolReactiveProperty(true);
        private readonly ReactiveProperty<bool> jumpDown = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> jumpUp = new ReactiveProperty<bool>(false);

        private readonly ReactiveProperty<float> move = new ReactiveProperty<float>(0f);

        public PlayerControlService()
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                move.Value = UnityEngine.Input.GetAxisRaw("Horizontal");
                jumpDown.Value = UnityEngine.Input.GetKeyDown(KeyCode.Space);
                jumpUp.Value = UnityEngine.Input.GetKeyUp(KeyCode.Space);
            }).AddTo(disposables);
        }

        public bool Enabled => enabled.Value;

        public void Enable()
        {
            enabled.Value = true;
        }

        public void Disable()
        {
            enabled.Value = false;
        }

        public IReadOnlyReactiveProperty<float> MoveAxis => move;
        public IReadOnlyReactiveProperty<bool> JumpPressed => jumpDown;
        public IReadOnlyReactiveProperty<bool> JumpReleased => jumpUp;

        public void Dispose()
        {
            disposables.Dispose();
            move.Dispose();
            jumpDown.Dispose();
            jumpUp.Dispose();
        }
    }
}