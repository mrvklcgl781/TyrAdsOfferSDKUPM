using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TyrDK
{
    internal class TyrOfferSDKService : TyrMonoService<TyrOfferSDKService>
    {
        [SerializeField] private TyrOfferView offerView;
        private CampaignData _campaignData;
        private string _userId;
        private TyrOfferApi API => TyrOfferApi.Instance;

        protected override bool IsGlobal => true;
        public void InitializeSDK()
        {
            string pUserId = PlayerPrefs.GetString("TyrUserId", string.Empty);
            if (string.IsNullOrEmpty(pUserId))
            {
                _userId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString("TyrUserId", _userId);
            }
            else
            {
                _userId = pUserId;
            }
            TyrAdsConfigService.Instance.SetData(_userId);
            if (API.ProcessType != TyrOfferApiProcessType.Initialize) return;
            if (API.Process == TyrOfferApiProcess.NotStarted)
                API.InitializeApi();
        }

        public void ShowOfferWall(Action<List<CampaignData>> onSuccess = null)
        {
            Debug.Log($"Showing Offer Wall for User ID: {_userId}");
            if (API.ProcessType == TyrOfferApiProcessType.Initialize)
            {
                if(API.Process == TyrOfferApiProcess.NotStarted)
                {
                    API.OnSdkInitialized += (res) => API.GetCampaigns();
                    API.OnCampaignsReceived += onSuccess;
                    API.GetCampaigns();
                }
                else if (API.Process == TyrOfferApiProcess.InProgress)
                {
                    API.OnSdkInitialized += (res) => API.GetCampaigns();
                    API.OnCampaignsReceived += onSuccess;
                }
                else if (API.Process == TyrOfferApiProcess.Completed)
                {
                    Debug.Log("SDK is already initialized, fetching campaigns.");
                    API.OnCampaignsReceived += onSuccess;
                }
            }
        }
        
        public void ShowOfferWallDetails(int campaignId)
        {
            Debug.Log($"Showing Offer Wall for User ID: {_userId}, Campaign ID: {campaignId}");
            if (API.ProcessType == TyrOfferApiProcessType.Initialize)
            {
                if(API.Process == TyrOfferApiProcess.NotStarted)
                {
                    API.OnSdkInitialized += (res) => API.GetCampaignDetails(campaignId);
                    API.OnCampaignDetailsReceived += (data) =>CreateOfferView(data);
                    InitializeSDK();
                }
                else if (API.Process == TyrOfferApiProcess.InProgress)
                {
                    API.OnSdkInitialized += (res) => API.GetCampaignDetails(campaignId);
                    API.OnCampaignDetailsReceived += (data) =>CreateOfferView(data);
                }
                else if (API.Process == TyrOfferApiProcess.Completed)
                {
                    Debug.Log("SDK is already initialized, fetching campaign details.");
                    API.OnCampaignDetailsReceived += (data) =>CreateOfferView(data);
                    API.GetCampaignDetails(campaignId);
                }
            }
            else if (API.ProcessType == TyrOfferApiProcessType.GetCampaigns)
            {
                if(API.Process == TyrOfferApiProcess.NotStarted)
                {
                    API.OnCampaignsReceived += (res) => API.GetCampaignDetails(campaignId);
                    API.OnCampaignDetailsReceived += (data) =>CreateOfferView(data);
                    API.GetCampaigns();
                }
                else if (API.Process == TyrOfferApiProcess.InProgress)
                {
                    API.OnCampaignsReceived += (res) => API.GetCampaignDetails(campaignId);
                    API.OnCampaignDetailsReceived += (data) =>CreateOfferView(data);
                }
                else if (API.Process == TyrOfferApiProcess.Completed)
                {
                    API.OnCampaignDetailsReceived += (data) =>CreateOfferView(data);
                    API.GetCampaignDetails(campaignId);
                }
            }
            else if (API.ProcessType == TyrOfferApiProcessType.GetCampaignDetails)
            {
                Debug.LogError("Already fetching campaign details.");
            }
        }
        
        private async Task CreateOfferView(CampaignData data)
        {
            offerView.gameObject.SetActive(true);
            _campaignData = data;
            var imageDownloader = new TyrDownloadService();
            Sprite icon = await imageDownloader.DownloadImage(_campaignData.app.thumbnail);
            Sprite pointIcon = await imageDownloader.DownloadImage(_campaignData.currency.adUnitCurrencyIcon);
            TyrViewConfig config = new TyrViewConfig()
            {
                AppIcon = icon,
                AppName = _campaignData.app.title,
                AppGenre = _campaignData.app.storeCategory,
                AppDifficulty = _campaignData.targeting.reward.rewardDifficulty,
                PointAmount = _campaignData.campaignPayout.totalPayout,
                PointName = _campaignData.currency.adUnitCurrencyName,
                PointIcon = pointIcon,
                RwdAmount = _campaignData.campaignPayout.totalEvents,
                PlaytimeTaskViews = new List<TyrTaskViewConfig>(),
                PayoutTaskViews = new List<TyrTaskViewConfig>(),
                MicroChargeEventViews = new List<TyrMicroChargeEventViewConfig>(),
                ClickUrl = _campaignData.tracking.clickUrl
            };
            foreach (var item in _campaignData.playtimeEvents)
            {
                config.PlaytimeTaskViews.Add(new TyrTaskViewConfig()
                {
                    Id = item.id,
                    PointIcon = pointIcon,
                    IsActive = item.conversionStatus == "approved",
                    MainText = "Play " + TimeSpan.FromSeconds(item.timePlayedSeconds).GetMinutes(),
                    PointText = item.payoutAmountConverted + " Points",
                    CompleteText = TimeSpan.FromSeconds(item.payoutAmountConverted).GetMinutes() + " Played"
                });
            }
            
            foreach (var item in _campaignData.payoutEvents)
            {
                config.PayoutTaskViews.Add(new TyrTaskViewConfig()
                {
                    Id = item.id,
                    PointIcon = pointIcon,
                    IsActive = item.conversionStatus == "approved",
                    MainText = item.eventName,
                    EventCategory = item.eventCategory,
                    PointText = item.payoutAmountConverted + " Points",
                    CompleteText = item.payoutAmountConverted + " Played"
                });
            }
            
            foreach (var item in _campaignData.microChargeEvents)
            {
                config.MicroChargeEventViews.Add(new TyrMicroChargeEventViewConfig()
                {
                    MainText = item.eventName,
                    TimeLimit = item.maxTimeRemainSeconds,
                    PointAmount = item.payoutAmountConverted,
                    PointIcon = pointIcon,
                    ProgressAmount = (float)item.count/(float)item.dailyCount,
                    ProgressText = item.eventCategory,
                    ProgressAmountText = item.count+"/" +item.dailyCount
                });
            }
            
            config.SortPlaytimeTaskViews();
            config.SortPayoutTaskViews();
            
            offerView.SetView(config);
        }

    }
}