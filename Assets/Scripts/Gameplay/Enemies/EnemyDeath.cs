using System;
using Systems.Combat;
using Systems.Input;
using Gameplay.Actors;
using UniRx;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyDeath : IInitializable, IDisposable
    {
        private readonly IDamageAnimator anim;
        private readonly IMovementControl control;
        private readonly CompositeDisposable d = new CompositeDisposable();
        private readonly IDamageable damageable;
        private readonly IEnemyView view;

        public EnemyDeath(IDamageable damageable, IDamageAnimator anim, IEnemyView view, IMovementControl control)
        {
            this.damageable = damageable;
            this.anim = anim;
            this.view = view;
            this.control = control;
        }

        public void Dispose()
        {
            d.Dispose();
        }

        public void Initialize()
        {
            damageable.Died.Subscribe(_ =>
            {
                control.Disable();
                view.Death();
            });

            damageable.Died
                .SelectMany(_ => { return anim.DeathFinished.Take(1); })
                .Subscribe(_ => view.Destroy())
                .AddTo(d);
        }
    }
}