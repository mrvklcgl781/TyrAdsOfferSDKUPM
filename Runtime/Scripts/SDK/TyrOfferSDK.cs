using System;
using System.Collections.Generic;

namespace TyrDK
{
    public class TyrOfferSDK : TyrClassService<TyrOfferSDK>
    {
        public void InitializeSDK()
        {
            if (TyrOfferSDKService.Instance== null)
            {
                TyrOfferSDKService.OnServiceInitialized += ()=>TyrOfferSDKService.Instance.InitializeSDK();
            }
            else
            {
                TyrOfferSDKService.Instance.InitializeSDK();
            }
        }
        
        public void ShowOfferWall(Action<List<CampaignData>> onCampaignReceived)
        {
            if (TyrOfferSDKService.Instance== null)
            {
                TyrOfferSDKService.OnServiceInitialized += ()=>TyrOfferSDKService.Instance.ShowOfferWall(onCampaignReceived);
            }
            else
            {
                TyrOfferSDKService.Instance.ShowOfferWall(onCampaignReceived);
            }
        }

        public void ShowOfferWallDetails(int campaignId)
        {
            if (TyrOfferSDKService.Instance== null)
            {
                TyrOfferSDKService.OnServiceInitialized += ()=>TyrOfferSDKService.Instance.ShowOfferWallDetails(campaignId);
            }
            else
            {
                TyrOfferSDKService.Instance.ShowOfferWallDetails(campaignId);
            }
        }
    }
}