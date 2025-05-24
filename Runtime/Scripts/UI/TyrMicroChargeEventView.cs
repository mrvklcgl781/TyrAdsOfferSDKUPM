using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TyrDK
{
    public class TyrMicroChargeEventView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI pointText, bonusPointText;
        [SerializeField] private Image pointIcon;
        [SerializeField] private TextMeshProUGUI timeLimitText;
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI progressText, progressAmountText;
        
        
        public void SetData(TyrMicroChargeEventViewConfig config)
        {
            mainText.text = config.MainText;
            pointText.text = config.PointAmount.ToString() + " Points";
            bonusPointText.text = config.BonusAmount.HasValue ? "+"+config.BonusAmount.Value.ToString() + " Points" : "";
            pointIcon.sprite = config.PointIcon;
            timeLimitText.text = config.TimeLimit.HasValue ?"Complete within "+ TimeSpan.FromSeconds(config.TimeLimit.Value).GetFormattedTimeSpan() : "";
            if (progressBar != null)
            {
                progressBar.fillAmount = config.ProgressAmount;
                progressText.text = config.ProgressText;
                progressAmountText.text = config.ProgressAmountText;
            }
        }
        
    }
}