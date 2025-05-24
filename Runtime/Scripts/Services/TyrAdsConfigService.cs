using UnityEngine;

namespace TyrDK
{
    public class TyrAdsConfigService : TyrMonoService<TyrAdsConfigService>
    {
        [SerializeField] private TyrAdsConfig adsConfig;
        protected override bool IsGlobal => true;
        public TyrAdsConfigData AdsConfigData { get; private set; }
        
        public void SetData(string userId)
        {
            AdsConfigData = new TyrAdsConfigData(adsConfig, userId);
        }
        
    }
}