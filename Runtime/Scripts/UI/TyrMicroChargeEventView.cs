using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TyrDK
{
    public class TyrMicroChargeEventView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI bonusPointText;
        [SerializeField] private CoinTextView pointText;
        [SerializeField] private TextMeshProUGUI timeLimitText;
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI progressText, progressAmountText;
        [SerializeField] private RectTransform rectTransform;

        private bool _isInitialized;
        private bool _isReadyToInitialize;
        public event Action OnViewCreated;
        public event Action<Vector2> OnBottomRightPos;
        public event Func<RectTransform> OnGetLimitsPosition;

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
            ViewExtension.CreateLimitGO("BR", corners[3], parent,OnBottomRightPos);
            OnGetLimitsPosition = null;
        }

        public void SetData(TyrMicroChargeEventViewConfig config)
        {
            mainText.text = config.MainText;
            if(bonusPointText != null)
            {
                bonusPointText.text =
                    config.BonusAmount.HasValue ? "+" + config.BonusAmount.Value.ToString() + " Points" : "";
            }
            pointText.SetView(config.PointIcon, config.PointAmount, config.CurrencyName);
            if(timeLimitText!= null)
            {
                timeLimitText.text = config.TimeLimit.HasValue
                    ? "Complete within " + TimeSpan.FromSeconds(config.TimeLimit.Value).GetFormattedTimeSpan()
                    : "";
            }
            if (progressBar != null)
            {
                progressBar.fillAmount = config.ProgressAmount;
                progressText.text = config.ProgressText;
                progressAmountText.text = config.ProgressAmountText;
            }
        }
    }
}