using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TyrDK
{
    public class TyrOfferView : MonoBehaviour
    {
        [Header("App Title View")]
        [SerializeField] private Image appIcon;
        [SerializeField] private TextMeshProUGUI appName;
        [SerializeField] private TextMeshProUGUI appGenre;
        [SerializeField] private TextMeshProUGUI appDifficulty;
        [SerializeField] private Image tPointIcon;
        [SerializeField] private TextMeshProUGUI tPointAmount;
        [SerializeField] private TextMeshProUGUI tPointName;
        [SerializeField] private TextMeshProUGUI tRwdAmount;
        
        [Header("App Image View")]
        [SerializeField] private Image appLargeImage;
        [SerializeField] private Button playButton;
        
        [Header("Active Tasks View")]
        [SerializeField] private RectTransform activeTasksViewParent, completedTasksViewParent;
        [SerializeField] private TyrTaskView playtimeTaskViewPrefab, payoutTaskViewPrefab;
        [SerializeField] private RectTransform activeTaskArea;
        
        [Header("Micro Charge View")]
        [SerializeField] private RectTransform microChargeViewParent,eventsViewParent;
        [SerializeField] private TyrMicroChargeEventView timelimPurchaseViewPrefab, purchaseViewPrefab, progressViewPrefab;
        
        private string _clickUrl;
        private bool _isViewSet;

        public void SetView(TyrViewConfig config)
        {
            appIcon.sprite = config.AppIcon;
            appName.text = config.AppName;
            appGenre.text = config.AppGenre;
            appDifficulty.text = config.AppDifficulty;
            tPointIcon.sprite = config.PointIcon;
            tPointAmount.text = config.PointAmount.ToString();
            tRwdAmount.text = config.RwdAmount.ToString();
            tPointName.text = config.PointName;
            appLargeImage.sprite = config.AppLargeImage;
            _clickUrl = config.ClickUrl;
            
            playButton.onClick.AddListener(OnClickPlayButton);
            foreach (var task in config.PlaytimeTaskViews)
            {
                TyrTaskView taskView;
                if (Mathf.Approximately(task.Progress, 1))
                {
                    taskView = Instantiate(playtimeTaskViewPrefab, completedTasksViewParent);
                }
                else
                {
                    taskView = Instantiate(playtimeTaskViewPrefab, activeTasksViewParent);
                }
                
                taskView.SetData(task);
            }
            
            foreach (var task in config.PayoutTaskViews)
            {
                TyrTaskView taskView;
                if (Mathf.Approximately(task.Progress, 1))
                {
                    taskView = Instantiate(payoutTaskViewPrefab, completedTasksViewParent);
                }
                else
                {
                    taskView = Instantiate(payoutTaskViewPrefab, activeTasksViewParent);
                }
                taskView.SetData(task);
            }

            microChargeViewParent.anchoredPosition = activeTaskArea.anchoredPosition + new Vector2(
                (activeTaskArea.rect.width / 2f - microChargeViewParent.rect.width / 2f)/2f + 22, 0);
            if (config.MicroChargeEventViews.Count == 0)
            {
                microChargeViewParent.gameObject.SetActive(false);
            }
            foreach (var task in config.MicroChargeEventViews)
            {
                TyrMicroChargeEventView taskView = null;
                if (task.TimeLimit.HasValue)
                {
                    taskView = Instantiate(timelimPurchaseViewPrefab, microChargeViewParent);
                }
                else if (task.ProgressAmount > 0)
                {
                    taskView = Instantiate(progressViewPrefab, microChargeViewParent);
                }
                else
                {
                    taskView = Instantiate(purchaseViewPrefab, eventsViewParent);
                }
                
                taskView.SetData(task);
            }
            _isViewSet = true;
        }
        
        private void OnClickPlayButton()
        {
            if (!string.IsNullOrEmpty(_clickUrl))
            {
                Application.OpenURL(_clickUrl);
            }
            else
            {
                Debug.LogError("Click URL is not set.");
            }
        }

        private void OnDestroy()
        {
            if (_isViewSet)
            {
                playButton.onClick.RemoveListener(OnClickPlayButton);
            }
        }
    }
}