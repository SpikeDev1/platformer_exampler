using System;
using Systems.Combat;
using Systems.Input;
using Systems.Movement;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Gameplay.Players.FSM
{
    /// <summary>
    ///     reset character and play spawn animation
    /// </summary>
    public class SpawnState : PlayerStateBase
    {
        private readonly IMovementControl control;
        private readonly IHealth health;
        private readonly IPlayerSpawnerView spawnerView;

        public SpawnState(IMovementControl control, IHealth health, IPlayerSpawnerView spawnerView)
        {
            this.control = control;
            this.health = health;
            this.spawnerView = spawnerView;
        }

        public override PlayerStateId Id => PlayerStateId.Spawn;

        public override async UniTask Enter()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.01), cancellationToken: ct);

            base.Enter().Forget();

            control.Disable();
            health.HealFull();
            spawnerView.Respawn();
            spawnerView.RespawnFinished.Take(1).Subscribe(_ => TransitToStay().Forget()).AddTo(d);
            await UniTask.CompletedTask;
        }

        public UniTask TransitToStay()
        {
            return TransitTo(PlayerStateId.Stay);
        }
    }

    /// <summary>
    /// </summary>
    public class FallState : PlayerStateBase
    {
        private readonly IRigidbody2DAdapter body;
        private readonly IMovementControl control;
        private readonly IDeath death;
        private readonly IJump jump;

        public FallState(IMovementControl control, IRigidbody2DAdapter body, IDeath death, IJump jump)
        {
            this.control = control;
            this.body = body;
            this.death = death;
            this.jump = jump;
        }

        public override PlayerStateId Id => PlayerStateId.Fall;

        public override UniTask Enter()
        {
            base.Enter().Forget();

            control.Enable();
            jump.Disable();

            return UniTask.CompletedTask;
        }

        public UniTask TransitToDeath()
        {
            return TransitTo(PlayerStateId.Death);
        }

        public UniTask TransitToStay()
        {
            jump.Enable();
            return TransitTo(PlayerStateId.Stay);
        }

        public UniTask TransitToRun()
        {
            return TransitTo(PlayerStateId.Run);
        }

        public override void Tick()
        {
            if (control.MoveAxis.Value != 0f)
            {
                TransitToRun();
            }

            if (body.IsGrounded == false)
            {
                TransitToStay();
            }

            if (death.IsAlive == false)
            {
                TransitToDeath();
            }
        }
    }

    public class StayState : PlayerStateBase
    {
        private readonly IRigidbody2DAdapter body;

        private readonly IMovementControl control;
        private readonly IDeath death;

        public StayState(IMovementControl control, IRigidbody2DAdapter body, IDeath death)
        {
            this.control = control;
            this.body = body;
            this.death = death;
        }

        public override PlayerStateId Id => PlayerStateId.Stay;

        public override UniTask Enter()
        {
            base.Enter().Forget();

            control.Enable();

            return UniTask.CompletedTask;
        }

        public override void Tick()
        {
            if (body.IsGrounded == false)
            {
                TransitToFall();
            }

            if (death.IsAlive == false)
            {
                TransitToDeath();
            }
        }

        public UniTask TransitToFall()
        {
            return TransitTo(PlayerStateId.Fall);
        }

        public UniTask TransitToDeath()
        {
            control.Disable();

            return TransitTo(PlayerStateId.Death);
        }
    }

    public class RunState : PlayerStateBase
    {
        private readonly IRigidbody2DAdapter body;

        private readonly IMovementControl control;
        private readonly IDeath death;

        public RunState(IMovementControl control, IRigidbody2DAdapter body, IDeath death)
        {
            this.control = control;
            this.body = body;
            this.death = death;
        }

        public override PlayerStateId Id => PlayerStateId.Run;

        public override UniTask Enter()
        {
            base.Enter().Forget();

            control.Enable();

            return UniTask.CompletedTask;
        }

        public override void Tick()
        {
            if (body.IsGrounded == false)
            {
                TransitToFall();
            }

            if (death.IsAlive == false)
            {
                TransitToDeath();
            }
        }

        public UniTask TransitToFall()
        {
            return TransitTo(PlayerStateId.Fall);
        }

        public UniTask TransitToDeath()
        {
            control.Disable();

            return TransitTo(PlayerStateId.Death);
        }
    }

    public class DeathState : PlayerStateBase
    {
        private readonly IRigidbody2DAdapter body;
        private readonly SignalBus bus;

        private readonly IMovementControl control;
        private readonly IHealth health;
        private readonly IPlayerDamageFacade _playerDamageView;
        private readonly IPlayerSpawnerView spawner;

        public DeathState(
            IMovementControl control,
            IHealth health,
            IPlayerDamageFacade playerView,
            IRigidbody2DAdapter body,
            SignalBus bus,
            IPlayerSpawnerView spawner)
        {
            this.control = control;
            this.health = health;
            this._playerDamageView = playerView;
            this.body = body;
            this.bus = bus;
            this.spawner = spawner;
        }

        public override PlayerStateId Id => PlayerStateId.Death;

        public override UniTask Enter()
        {
            base.Enter().Forget();
            control.Disable();
            _playerDamageView.DeathFinished
                .Take(1)
                .Subscribe(_ => bus.Fire(new PlayerDiedSignal(body, _playerDamageView.Transform, spawner, fsm)))
                .AddTo(d);
            _playerDamageView.Death();

            return UniTask.CompletedTask;
        }

        public override void Tick()
        {
            if (fsm.Id == PlayerStateId.Spawn)
            {
                TransitTo(PlayerStateId.Spawn).Forget();
            }
        }
    }
}