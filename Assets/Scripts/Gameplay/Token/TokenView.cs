using Core.Utilities;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Token
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TokenView : MonoBehaviour
    {
        private const float defaultFps = 12f;
        private readonly CompositeDisposable d = new CompositeDisposable();
        private ISpriteAnimatorModel anim;
        public AudioClip collectAudio;
        public Sprite[] collectedAnimation;
        private ISpriteAnimatorControl control;
        public Sprite[] idleAnimation;
        private IPickupModel pickup;

        private SpriteRenderer r;
        public bool randomStart = false;

        [Inject]
        private void Construct([InjectOptional] IPickupModel pickup, [InjectOptional] ISpriteAnimatorModel anim,
            [InjectOptional] ISpriteAnimatorControl control)
        {
            this.pickup = pickup;
            this.anim = anim;
            this.control = control;

            if (pickup == null || anim == null || control == null) return;

            pickup.Collected.Subscribe(OnCollectedChanged).AddTo(d);
            anim.Frame.Subscribe(OnFrameChanged).AddTo(d);
            control.PlayLoopRandom(idleAnimation, defaultFps, randomStart);
        }

        private void Awake()
        {
            r = GetComponent<SpriteRenderer>();
        }

        private void OnCollectedChanged(bool isCollected)
        {
            if (isCollected)
            {
                if (collectAudio) AudioSource.PlayClipAtPoint(collectAudio, transform.position);
                var seq = collectedAnimation != null && collectedAnimation.Length > 0
                    ? collectedAnimation
                    : idleAnimation;
                control.Play(seq, defaultFps, 0);
            }
            else
            {
                control.PlayLoopRandom(idleAnimation, defaultFps, randomStart);
            }
        }

        private void OnFrameChanged(int frame)
        {
            var seq = pickup.Collected.Value
                ? collectedAnimation != null && collectedAnimation.Length > 0 ? collectedAnimation : idleAnimation
                : idleAnimation;
            if (seq == null || seq.Length == 0) return;
            if ((uint) frame >= (uint) seq.Length) return;
            r.sprite = seq[frame];
            if (pickup.Collected.Value && frame == seq.Length - 1) Destroy(gameObject);
        }

        private void OnDestroy()
        {
            d.Dispose();
        }
    }
}