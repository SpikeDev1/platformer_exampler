using System;
using System.Collections.Generic;
using Core.Utilities;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Model.Configs
{
    [CreateAssetMenu(menuName = "Audio/SfxBank")]
    public class SfxBankSO : ScriptableObjectInstaller<SfxBankSO>, ISfxBank
    {
        public Entry[] items;

        private Dictionary<string, Entry> map;

        public bool TryGetClip(string id, out AudioClip clip)
        {
            clip = null;
            if (string.IsNullOrEmpty(id)) return false;
            if (!map.TryGetValue(id, out var e) || e.clips == null || e.clips.Length == 0) return false;
            clip = e.clips[e.clips.Length == 1 ? 0 : Random.Range(0, e.clips.Length)];
            return clip != null;
        }

        private void OnEnable()
        {
            map = new Dictionary<string, Entry>(StringComparer.Ordinal);
            if (items == null) return;
            for (var i = 0; i < items.Length; i++)
                if (!string.IsNullOrEmpty(items[i].id))
                    map[items[i].id] = items[i];
        }

        public override void InstallBindings()
        {
            Container.Bind<ISfxBank>().FromInstance(this).AsSingle();
        }

        [Serializable]
        public class Entry
        {
            public AudioClip[] clips;
            public string id;
        }
    }
}