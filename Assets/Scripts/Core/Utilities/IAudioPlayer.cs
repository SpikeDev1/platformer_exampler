using UnityEngine;

namespace Core.Utilities
{
    public interface IAudioPlayer
    {
        void PlayOneShot(AudioClip clip);
        void PlayOneShot(string idAudio);
    }
}