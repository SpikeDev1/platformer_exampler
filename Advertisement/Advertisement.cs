using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gamemaker.Advertisement
{
    public enum TypeAds
    {
        REWARD,
        INTER
    }

    [System.Serializable]
    public class AdsKeys
    {
        public List<AdsKey> keys = new List<AdsKey>();
    }

    [System.Serializable]
    public class AdsKey
    {
        public string placement;
        public TypeAds type;
        public string key;
    }

    public interface IAdertisementBridge
    {
        
    }

    public interface IMyAdvertisement
    {
        float IntervalShowInterAds { get; set; }

        bool IsOpen { get; }

        bool IsLoading(string kindAds);

        bool IsLoaded(string kindAds);

        bool IsCanShow(string kindAds);

        void Load(string place, string kindAds, Action onLoaded, Action<string> onFail);

        void Show(string place, string kindAds, Action onFinished, Action onClosed);

        void Update();
    }

    public class Advertisement : IMyAdvertisement
    {
        private IAdertisementBridge bridge;

        public Advertisement(IAdertisementBridge bridge)
        {
            this.bridge = bridge;
        }

        public bool IsLoaded(string kindAds)
        {
            return true;
        }

        public bool IsCanShow(string kindAds)
        {
            throw new NotImplementedException();
        }

        public void Load(string place, string kindAds, Action onLoaded, Action<string> onFail)
        {
            throw new NotImplementedException();
        }

        public void Show(string place, string kindAds, Action onFinished, Action onClosed)
        {
            throw new NotImplementedException();
        }

        public float IntervalShowInterAds { get; set; }

        public bool IsOpen { get; }

        public bool IsLoading(string kindAds)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
        }
    }
}
