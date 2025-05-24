namespace TyrDK
{
    public struct TyrAdsConfigData
    {
        public string apiKey;
        public string apiSecret;
        public string userId;
        public string apiHost;
        public string apiVersion;
        public string language;

        public string sdkVersion;
        public string sdkPlatform;

        public TyrAdsConfigData(TyrAdsConfig config, string pUserId)
        {
            apiKey = config.ApiKey;
            apiSecret = config.ApiSecret;
            userId = pUserId;
            apiHost = config.ApiHost;
            apiVersion = config.ApiVersion;
            language = config.Language;
            sdkVersion = config.SdkVersion;
            sdkPlatform = config.SdkPlatform;
        }
    }
}