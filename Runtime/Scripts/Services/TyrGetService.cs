using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace TyrDK
{
    public abstract class TyrGetService<T> : TyrClassService<T> where T : TyrGetService<T>, new()
    {
        protected TyrAdsConfigData ConfigData =>TyrAdsConfigService.Instance.AdsConfigData;

        protected IEnumerator Get(string url,Action<string> onSuccess, Action<string> onError)
        {
            if (string.IsNullOrEmpty(ConfigData.apiKey) || string.IsNullOrEmpty(ConfigData.apiSecret) ||
                string.IsNullOrEmpty(ConfigData.userId))
            {
                onError?.Invoke("API credentials are not set. Make sure apiKey, apiSecret, and userId are provided.");
                yield break;
            }
            
            Debug.Log($"Fetching campaigns from: {url}");

            var webService = TyrWebRequestService.Instance;
            webService.CreateGetWebRequest(url);

            webService.AddHeader("X-API-Key", ConfigData.apiKey);
            webService.AddHeader("X-API-Secret", ConfigData.apiSecret);
            webService.AddHeader("X-User-ID", ConfigData.userId);
            webService.AddHeader("X-SDK-Platform", ConfigData.sdkPlatform);
            webService.AddHeader("X-SDK-Version", ConfigData.sdkVersion);
            webService.AddHeader("User-Agent", SystemInfo.operatingSystem);

            yield return webService.SendRequest(onSuccess, onError);
        }
    }
}