using UnityEngine;
using Zenject;

namespace Core.Utilities
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceAdapter : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private AudioSource audioSource;
        private ISfxBank bank;

        public void PlayOneShot(AudioClip clip)
        {
            if (clip) audioSource.PlayOneShot(clip);
        }

        public void PlayOneShot(string idAudio)
        {
            if (string.IsNullOrEmpty(idAudio)) return;

            if (bank != null && bank.TryGetClip(idAudio, out var clip)) audioSource.PlayOneShot(clip);
            else Debug.LogWarning($"SFX id not found: {idAudio}");
        }

        [Inject]
        private void Construct(ISfxBank bank)
        {
            this.bank = bank;
        }

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}