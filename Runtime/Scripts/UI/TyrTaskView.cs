using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TyrDK
{
    public class TyrTaskView : MonoBehaviour
    {
        [SerializeField] private Image activeBorder;
        [SerializeField] private Image progressBar;
        [SerializeField] private Image completeImage;
        [SerializeField] private Image pointIcon;
        [SerializeField] private Image taskIcon;
        [SerializeField] private TextMeshProUGUI mainText,pointText, completeText;
        [SerializeField] private TextMeshProUGUI eventCategoryText;
        
        public void SetData(TyrTaskViewConfig config)
        {
            if (Mathf.Approximately(config.Progress, 1))
            {
                progressBar.fillAmount = 1f;
                completeText.text = "Completed";
                completeText.color = Color.white;
                completeImage.enabled = true;
            }
            else
            {
                progressBar.fillAmount = config.Progress;
                completeText.text = config.CompleteText;
            }
            pointIcon.sprite = config.PointIcon;
            mainText.text = config.MainText;
            pointText.text = config.PointText;
            activeBorder.enabled = config.IsActive;
            if (!config.IsActive)
            {
                completeText.text = "Not available yet";
            }
            if (eventCategoryText != null)
            {
                eventCategoryText.text = config.EventCategory;
            }
        }
    }
}