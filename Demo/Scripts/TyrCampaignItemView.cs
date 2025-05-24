using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class TyrCampaignItemView : MonoBehaviour
    {
        [SerializeField] private Button campaignButton;
        [SerializeField] private TextMeshProUGUI campaignIdText;
        
        private int _campaignId;
        
        public void SetData(int campaignId,Action<int> onClick)
        {
            _campaignId = campaignId;
            campaignIdText.text = "Campaign ID: " + campaignId;
            campaignButton.onClick.RemoveAllListeners();
            campaignButton.onClick.AddListener(() =>
            {
                onClick?.Invoke(campaignId);
            });
        }
    }
}