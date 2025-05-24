using UnityEngine;

namespace TyrDK
{
    [CreateAssetMenu(fileName = "TyrAdsConfig", menuName = "TyrAds/Configuration")]
    public class TyrAdsConfig : ScriptableObject
    {
        [Header("API Configuration")]
        [SerializeField] private string apiKey;
        [SerializeField] private string apiSecret;
        [SerializeField] private string defaultUserId = "default_user";
        [SerializeField] private string apiHost = "https://api.tyrads.com";
        [SerializeField] private string apiVersion = "v1";
        [SerializeField] private string language = "en";
        
        [Header("SDK Configuration")]
        [SerializeField] private string sdkVersion = "1.0.0";
        [SerializeField] private string sdkPlatform = "Android";
        
        public string ApiKey => apiKey;
        public string ApiSecret => apiSecret;
        public string DefaultUserId => defaultUserId;
        public string ApiHost => apiHost;
        public string ApiVersion => apiVersion;
        public string Language => language;
        public string SdkVersion => sdkVersion;
        public string SdkPlatform => sdkPlatform;
        
        public void SetCredentials(string apiKey, string apiSecret)
        {
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
        }
        
        public void SetUserId(string userId)
        {
            this.defaultUserId = userId;
        }
        
        public void SetLanguage(string language)
        {
            this.language = language;
        }
    }
}
