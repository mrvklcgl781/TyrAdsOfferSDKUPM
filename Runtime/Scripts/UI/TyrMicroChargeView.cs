using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TyrDK
{
    public class TyrMicroChargeView : MonoBehaviour
    {
        [SerializeField] private RectTransform scrollArea;
        [SerializeField] private RectTransform microChargeViewParent, eventsViewParent, areaParent;

        [SerializeField] private TyrMicroChargeEventView timelimPurchaseViewPrefab, purchaseViewPrefab, progressViewPrefab;
        [SerializeField] private TextMeshProUGUI titlePointsText1, titlePointsText2;

        private Action _onViewCreateComplete;
        public Vector2 BottomRightPosition { get; private set; }
        public Vector2 TopLeftPosition { get; private set; }

        public void SetView(List<TyrMicroChargeEventViewConfig> microChargeEventViews, Action onViewCreateComplete, string currencyName)
        {
            _onViewCreateComplete = onViewCreateComplete;
            Vector3[] corners = new Vector3[4];
            microChargeViewParent.GetWorldCorners(corners);
            ViewExtension.CreateLimitGO("TL", corners[1], GetParent(), SetTop);
            
            int totalPoints = 0;
            for (int i = 0; i < microChargeEventViews.Count; i++)
            {
                var task = microChargeEventViews[i];
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
                taskView.OnViewCreated += OnViewCreated;
                if (i == microChargeEventViews.Count - 1)
                {
                    taskView.OnGetLimitsPosition += GetParent;
                    taskView.OnBottomRightPos += SetBottom;
                }
                taskView.ReadyToInitialize();
                taskView.SetData(task);
                totalPoints += task.PointAmount;
            }
            
            titlePointsText1.text += CoinTextView.FormatText(totalPoints);
            titlePointsText2.text += CoinTextView.FormatText(totalPoints) + " " + currencyName;
        }

        private void OnViewCreated()
        {
            float height = Mathf.Abs(BottomRightPosition.y - TopLeftPosition.y);
            areaParent.sizeDelta = new Vector2(areaParent.sizeDelta.x, height);
            _onViewCreateComplete?.Invoke();
        }

        private void SetBottom(Vector2 pos)
        {
            if (pos.y < 0)
            {
                pos.y -= 68f;
            }
            else
            {
                pos.y += 68f;
            }
            BottomRightPosition = pos;
        }

        private void SetTop(Vector2 pos)
        {
            TopLeftPosition = pos;
        }

        private RectTransform GetParent()
        {
            return scrollArea;
        }
    }
}