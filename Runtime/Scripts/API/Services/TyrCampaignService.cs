using System;
using System.Collections.Generic;
using UnityEngine;


namespace TyrDK
{
    public class TyrCampaignService : TyrGetService<TyrCampaignService>
    {
        public void GetCampaignDetails(int campaignId,Action<CampaignData> onSuccess, Action<string> onError)
        {
	        string url = $"{ConfigData.apiHost}/{ConfigData.apiVersion}/campaigns/{campaignId}?lang={ConfigData.language}";
            TyrCoroutineService.Instance.Execute(Get(url,OnSuccess, onError));

            void OnSuccess(string jsonResponse)
            {
	            TyrCampaignDetails response = JsonUtility.FromJson<TyrCampaignDetails>(jsonResponse);
	            if (response == null)
	            {
		            onError?.Invoke("Failed to parse campaign response. Response is null.");
		            return;
	            }
                        
	            if (response.data == null)
	            {
		            Debug.LogWarning("Campaign response contains no data.");
	            }
                        
	            onSuccess?.Invoke(response.data);
            }
        }
        
        public void GetCampaigns(Action<List<CampaignData>> onSuccess = null, Action<string> onError = null)
		{
	        string url = $"{ConfigData.apiHost}/{ConfigData.apiVersion}/campaigns?lang={ConfigData.language}";
			TyrCoroutineService.Instance.Execute(Get(url,OnSuccess, onError));

			void OnSuccess(string jsonResponse)
			{
	            TyrCampaigns response = JsonUtility.FromJson<TyrCampaigns>(jsonResponse);
	            if (response == null)
	            {
		            onError?.Invoke("Failed to parse campaign response. Response is null.");
		            return;
	            }
						
	            if (response.data == null || response.data.Count == 0)
	            {
		            Debug.LogWarning("Campaign response contains no data.");
	            }
						
	            onSuccess?.Invoke(response.data);
			}
		}
    }
}