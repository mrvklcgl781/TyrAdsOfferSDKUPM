using System.Collections.Generic;
using TyrDK;
using UnityEngine;

namespace Demo
{
    public class DemoScene : TyrMonoService<DemoScene>
    {
        [SerializeField] private RectTransform scrollContent;
        [SerializeField] private TyrCampaignItemView campaignItemPrefab;
        protected override bool IsGlobal => false;
        
        protected override void Initialize()
        {
            TyrOfferSDK.Instance.InitializeSDK();
            TyrOfferSDK.Instance.ShowOfferWall(OnCampaignReceived);
        }
        
        private void OnCampaignReceived(List<CampaignData> campaignData)
        {
            Debug.Log($"Campaign Received: {campaignData}");
            foreach (var item in campaignData)
            {
                var campaignItem = Instantiate(campaignItemPrefab, scrollContent);
                campaignItem.SetData(item.campaignId,OnCampaignItemClicked);
            }
        }

        private void OnCampaignItemClicked(int campaignId)
        {
            gameObject.SetActive(false);
            TyrOfferSDK.Instance.ShowOfferWallDetails(campaignId, ()=>gameObject.SetActive(true));
        }
    }
}