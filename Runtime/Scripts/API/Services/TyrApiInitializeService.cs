using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Device;

namespace TyrDK
{
    public class TyrApiInitializeService : TyrClassService<TyrApiInitializeService>
    {
        private TyrAdsConfigData ConfigData =>TyrAdsConfigService.Instance.AdsConfigData;
        public void InitializeSDK(Action<string> onSuccess = null, Action<string> onError = null)
        {
            var deviceData = new Dictionary<string, object>
            {
                { "device", SystemInfo.deviceModel },
                { "brand", SystemInfo.deviceModel },
                { "model", SystemInfo.deviceModel },
                { "manufacturer", SystemInfo.deviceModel },
                { "product", Application.productName },
                { "host", SystemInfo.deviceName },
                { "hardware", SystemInfo.processorType },
                { "serialNumber", "unknown" },
                { "androidId", SystemInfo.deviceUniqueIdentifier },
                { "deviceAge", DateTime.UtcNow.ToString("o") },
                { "display", SystemInfo.operatingSystem },
                { "heightInches", (Screen.height / Screen.dpi).ToString() },
                { "widthInches", (Screen.width / Screen.dpi).ToString() },
                { "heightPx", Screen.height.ToString() },
                { "widthPx", Screen.width.ToString() },
                { "xdpi", Screen.dpi.ToString() },
                { "ydpi", Screen.dpi.ToString() },
                { "baseOs", SystemInfo.operatingSystem },
                { "codename", "REL" },
                { "type", "user" },
                { "tags", "release-keys" },
                { "build", Application.version },
                { "buildSign", "" },
                { "version", Application.version },
                { "package", Application.identifier },
                { "installerStore", "" },
                { "lang", Application.systemLanguage.ToString() },
                { "osLang", Application.systemLanguage.ToString() },
                { "rooted", false },
                { "isVirtual", false }  // Changed from 'virtual' to 'isVirtual'
            };

            var requestBody = new Dictionary<string, object>
            {
                { "platform", "Android" },
                { "identifierType", "GAID" },
                { "identifier", "GAID-USER-EXAMPLE-1" },
                { "publisherUserId", ConfigData.userId},
                { "email", "" },
                { "phoneNumber", "" },
                { "age", 0 },
                { "gender", 0 },
                { "sub1", "" },
                { "sub2", "" },
                { "sub3", "" },
                { "sub4", "" },
                { "sub5", "" },
                { "userGroup", null },
                { "mediaSourceName", null },
                { "mediaSourceId", null },
                { "mediaSubSourceId", null },
                { "incentivized", null },
                { "mediaAdsetName", null },
                { "mediaAdsetId", null },
                { "mediaCreativeName", null },
                { "mediaCreativeId", null },
                { "mediaCampaignName", null },
                { "deviceData", deviceData }
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);
            string url = $"{ConfigData.apiHost}/{ConfigData.apiVersion}/initialize";
            if(TyrCoroutineService.Instance !)
                TyrCoroutineService.Instance.Execute(Post(url,jsonBody,onSuccess,onError));
            else
            {
                TyrCoroutineService.AddToQueue(Post(url,jsonBody,onSuccess,onError));
            }
        }

        private IEnumerator Post(string url, string jsonBody,Action<string> onSuccess = null, Action<string> onError= null)
        {
            var webService = TyrWebRequestService.Instance;
            webService.CreatePostWebRequest(url, jsonBody);
            
            webService.AddHeader("Content-Type", "application/json");
            webService.AddHeader("X-API-Key", ConfigData.apiKey);
            webService.AddHeader("X-API-Secret", ConfigData.apiSecret);
            webService.AddHeader("X-SDK-Platform", ConfigData.sdkPlatform);
            webService.AddHeader("X-SDK-Version", ConfigData.sdkVersion);
            webService.AddHeader("X-Secure-Mode", "PLAIN");
            webService.AddHeader("X-Play-Integrity", "");

            yield return webService.SendRequest(onSuccess, onError);
        }
    }
}