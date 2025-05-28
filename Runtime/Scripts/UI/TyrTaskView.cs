using System;
using System.Collections;
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
        [SerializeField] private Image taskIcon;
        [SerializeField] private TextMeshProUGUI mainText, completeText;
        [SerializeField] private TextMeshProUGUI eventCategoryText;
        [SerializeField] private CoinTextView pointText;
        [SerializeField] private RectTransform rectTransform;
        
        private Color completedColor = new Color(247/255f, 214/255f, 72/255f, 1f); 
        
        public event Action OnViewCreated;
        public event Func<RectTransform> OnGetLimitsPosition;
        public event Action<Vector2> OnBottomRightPos, OnTopLeftPos;
        private bool _isInitialized = false;
        private Action _onStartAction;
        private bool _isReadyToInitialize;

        public void Start()
        {
            if (!_isInitialized && _isReadyToInitialize)
            {
                StartCoroutine(Initialize());
                _isInitialized = true;
            }
        }

        public void ReadyToInitialize()
        {
            _isReadyToInitialize = true;
            if (!_isInitialized)
            {
                StartCoroutine(Initialize());
                _isInitialized = true;
            }
        }

        private IEnumerator Initialize()
        {
            if (!_isInitialized)
            {
                if (OnGetLimitsPosition != null)
                {
                    yield return null;
                    SetLimits();
                }

                OnViewCreated?.Invoke();
            }
        }


        private void SetLimits()
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var parent = OnGetLimitsPosition?.Invoke();
            ViewExtension.CreateLimitGO("BottomRightPosition", corners[3], parent, OnBottomRightPos);
            ViewExtension.CreateLimitGO("TopLeftPosition", corners[1], parent,OnTopLeftPos);
            OnGetLimitsPosition = null;
        }

        public void SetData(TyrTaskViewConfig config)
        {
            if (Mathf.Approximately(config.Progress, 1))
            {
                progressBar.fillAmount = 1f;
                completeText.text = "Completed";
                completeText.color = Color.white;
                completeImage.enabled = true;
                taskIcon.color = completedColor;
            }
            else
            {
                progressBar.fillAmount = config.Progress;
                completeText.text = config.CompleteText;
            }
            pointText.SetView(config.PointIcon,config.PointAmount, config.CurrencyName);
            mainText.text = config.MainText;
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