using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace TyrDK
{
    public class TyrTaskAreaView : MonoBehaviour
    {
        [SerializeField] private RectTransform activeTasksViewParent, completedTasksViewParent;
        [SerializeField] private TyrTaskView playtimeTaskViewPrefab, payoutTaskViewPrefab;
        [SerializeField] private RectTransform scrollArea;
        public Vector2? BottomRightPosition { get; private set; }
        public Vector2? TopLeftPosition { get; private set; }

        public void SetView(List<TyrTaskViewConfig> playtimeTaskViews, List<TyrTaskViewConfig> payoutTaskViews,
            Action onViewCreateComplete)
        {
            int completedCount = 0;
            for (int i = 0; i < playtimeTaskViews.Count; i++)
            {
                CreateTaskView(playtimeTaskViews[i], activeTasksViewParent, playtimeTaskViewPrefab, ref completedCount,
                    onViewCreateComplete, i);
            }

            for (int i = 0; i < payoutTaskViews.Count; i++)
            {
                CreateTaskView(payoutTaskViews[i], activeTasksViewParent, payoutTaskViewPrefab, ref completedCount,
                    onViewCreateComplete,i);
            }

            if (completedCount == 0)
            {
                completedTasksViewParent.transform.parent.gameObject.SetActive(false);
            }
        }

        private void CreateTaskView(TyrTaskViewConfig task, RectTransform parent, TyrTaskView prefab,
            ref int completedCount, Action onViewCreateComplete, int index)
        {
            TyrTaskView taskView = null;
            if (Mathf.Approximately(task.Progress, 1))
            {
                taskView = Instantiate(prefab, completedTasksViewParent);
                completedCount++;
            }
            else
            {
                taskView = Instantiate(prefab, parent);
            }
            taskView.name = prefab.name +"_"+ index;

            taskView.OnViewCreated += () =>
            {
                onViewCreateComplete?.Invoke();
            };
            CheckLimits(taskView);
            taskView.SetData(task);
        }

        private void CheckLimits(TyrTaskView taskView)
        {
            taskView.OnTopLeftPos += SetTop;
            taskView.OnGetLimitsPosition += GetParent;
            taskView.OnBottomRightPos += SetBottom;
            taskView.ReadyToInitialize();
        }

        private void SetBottom(Vector2 pos)
        {
            if (BottomRightPosition.HasValue)
            {
                if (BottomRightPosition.Value.x < pos.x || BottomRightPosition.Value.y > pos.y)
                {
                    BottomRightPosition = pos;
                }
            }
            else
            {
                BottomRightPosition = pos;
            }
        }

        private void SetTop(Vector2 pos)
        {
            if (TopLeftPosition.HasValue)
            {
                if (TopLeftPosition.Value.x > pos.x || TopLeftPosition.Value.y < pos.y)
                {
                    TopLeftPosition = pos;
                }
            }
            else
            {
                TopLeftPosition = pos;
            }
        }

        private RectTransform GetParent()
        {
            return scrollArea;
        }
    }
}