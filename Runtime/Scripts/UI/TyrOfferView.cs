using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TyrDK
{
    public class TyrOfferView : MonoBehaviour
    {
        [SerializeField] private ConditionalScrollController scrollController;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform taskArea;
        [SerializeField] private Button backButton;
        
        [Header("App Title View")]
        [SerializeField] private Image appIcon;
        [SerializeField] private TextMeshProUGUI appName;
        [SerializeField] private TextMeshProUGUI appGenre;
        [SerializeField] private TextMeshProUGUI appDifficulty;
        [SerializeField] private TextMeshProUGUI tPointName;
        [SerializeField] private TextMeshProUGUI tRwdAmount;
        [SerializeField] private CoinTextView tRwdCoinText;
        
        [Header("App Image View")]
        [SerializeField] private Image appLargeImage;
        [SerializeField] private Button playButton;
        
        [Header("Active Tasks View")]
        [SerializeField] private TyrTaskAreaView activeTaskAreaView;
        [SerializeField] private RectTransform activeTaskArea;
        
        [Header("Micro Charge View")]
        [SerializeField] private TyrMicroChargeView microChargeView;
        [SerializeField] private RectTransform microChargeViewParent;
        
        private string _clickUrl;
        private bool _isViewSet;
        private bool _isConditionalScrollSet = false;

        private int _viewCount = 0, _completedViewCount = 0;
        public void SetView(TyrViewConfig config, Action onBackButtonClick)
        {
            appIcon.sprite = config.AppIcon;
            appName.text = config.AppName;
            appGenre.text = config.AppGenre;
            appDifficulty.text = config.AppDifficulty;
            tRwdCoinText.SetView(config.PointIcon, config.PointAmount,"");
            tRwdAmount.text = config.RwdAmount.ToString();
            tPointName.text = config.PointName;
            appLargeImage.sprite = config.AppLargeImage;
            _clickUrl = config.ClickUrl;
            backButton.onClick.AddListener(() =>
            {
                canvasGroup.alpha = 0f;
                onBackButtonClick?.Invoke();
            });
            
            playButton.onClick.AddListener(OnClickPlayButton);

            _viewCount = config.PlaytimeTaskViews.Count + config.PayoutTaskViews.Count + config.MicroChargeEventViews.Count;
            activeTaskAreaView.SetView(config.PlaytimeTaskViews, config.PayoutTaskViews, OnViewCreateComplete);

            if (config.MicroChargeEventViews.Count > 0)
            {
                _isConditionalScrollSet = true;
                microChargeView.SetView(config.MicroChargeEventViews, OnViewCreateComplete, config.PointName);
            }
            else
            {
                _isConditionalScrollSet = false;
                microChargeView.gameObject.SetActive(false);
            }
            
            _isViewSet = true;
        }

        private void OnViewCreateComplete()
        {
            _completedViewCount++;
            if (_completedViewCount >= _viewCount)
            {
                if (_isConditionalScrollSet)
                {
                    SetConditionalScroll();
                    microChargeView.gameObject.SetActive(true);
                }
                else
                {
                    SetRegularScroll();
                }
                canvasGroup.alpha = 1f;
                _completedViewCount = 0;
                _viewCount = 0;
            }
        }
        
        private void SetConditionalScroll()
        {
            float minX =activeTaskAreaView.TopLeftPosition.Value.x;
            float maxY = Mathf.Max(activeTaskAreaView.TopLeftPosition.Value.y, scrollController.GetContentAreaTopLeftPosition().y);
            float maxX = activeTaskAreaView.BottomRightPosition.Value.x;
            float minY = activeTaskAreaView.BottomRightPosition.Value.y;
            Vector2 size = new Vector2(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY));
            scrollController.SetConditional(size);
            var initPos = microChargeView.transform.position; 
            var microTransform = microChargeView.transform;
            scrollController.OnHorizontalScroll += () =>
            {
                microTransform.SetParent(taskArea);
                microTransform.position = new Vector3(initPos.x, microTransform.position.y, microTransform.position.z);
            };
            scrollController.OnVerticalScroll += () =>
            {
                microTransform.SetParent(scrollController.Content);
                microTransform.position = new Vector3(initPos.x, microTransform.position.y, microTransform.position.z);
            };
            ViewExtension.ClearLimitGOs();
        }

        private void SetRegularScroll()
        {
            float minX = Mathf.Min(activeTaskAreaView.TopLeftPosition.Value.x, microChargeView.TopLeftPosition.x);
            float maxY = Mathf.Max(activeTaskAreaView.TopLeftPosition.Value.y, microChargeView.TopLeftPosition.y);
            maxY = Mathf.Max(maxY, scrollController.GetContentAreaTopLeftPosition().y);
            float maxX = Mathf.Max(activeTaskAreaView.BottomRightPosition.Value.x, microChargeView.BottomRightPosition.x);
            float minY = Mathf.Min(activeTaskAreaView.BottomRightPosition.Value.y, microChargeView.BottomRightPosition.y);
            Vector2 size = new Vector2(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY));
            scrollController.SetRegularHorizontalScroll(Vector2.zero, size);
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