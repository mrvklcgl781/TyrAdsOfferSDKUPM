using System;
using System.Collections.Generic;
using UnityEngine;

namespace TyrDK
{
    internal class TyrOfferApi : TyrClassService<TyrOfferApi>
    {
        public event Action<string> OnSdkInitialized;
        public event Action<List<CampaignData>> OnCampaignsReceived;
        public event Action<CampaignData> OnCampaignDetailsReceived;
        public TyrOfferApiProcess Process { get; private set; } = TyrOfferApiProcess.NotStarted;
        public TyrOfferApiProcessType ProcessType { get; private set; } = TyrOfferApiProcessType.Initialize;
        
        public void InitializeApi()
        {
            Process = TyrOfferApiProcess.InProgress;
            ProcessType = TyrOfferApiProcessType.Initialize;
            TyrApiInitializeService.Instance.InitializeSDK(OnSuccessInit,OnError);
        }
        public void GetCampaigns()
        {
            Process = TyrOfferApiProcess.InProgress;
            ProcessType = TyrOfferApiProcessType.GetCampaigns;
            TyrCampaignService.Instance.GetCampaigns(OnSuccessGetCampaigns,OnError);
        }
        
        public void GetCampaignDetails(int campaignId)
        {
            Process = TyrOfferApiProcess.InProgress;
            ProcessType = TyrOfferApiProcessType.GetCampaignDetails;
            TyrCampaignService.Instance.GetCampaignDetails(campaignId, OnSuccessGetCampaignDetails, OnError);
        }
        
        private void OnSuccessInit(string response)
        {
            Process = TyrOfferApiProcess.Completed;
            OnSdkInitialized?.Invoke(response);
        }
        
        private void OnSuccessGetCampaigns(List<CampaignData> response)
        {
            Process = TyrOfferApiProcess.Completed;
            if (response == null)
            {
                OnCampaignsReceived?.Invoke(null);
                return;
            }

            if (response.Count == 0)
            {
                OnCampaignsReceived?.Invoke(new List<CampaignData>());
                return;
            }

            OnCampaignsReceived?.Invoke(response);
        }
        
        private void OnSuccessGetCampaignDetails(CampaignData data)
        {
            Process = TyrOfferApiProcess.Completed;
            if (data == null)
            {
                OnCampaignDetailsReceived?.Invoke(null);
                return;
            }

            OnCampaignDetailsReceived?.Invoke(data);
        }

        private void OnError(string error)
        {
            Process = TyrOfferApiProcess.Completed;
            Debug.LogError(error);
        }
        
        public void ResetCampaignDetails()
        {
            Process = TyrOfferApiProcess.NotStarted;
            OnCampaignDetailsReceived = null;
        }
    }
    
    public enum TyrOfferApiProcess
    {
        NotStarted =0,
        InProgress =1,
        Completed =2,
    }

    public enum TyrOfferApiProcessType
    {
        None=0,
        Initialize = 1,
        GetCampaigns = 2,
        GetCampaignDetails = 3,
    }
}