using System.Collections.Generic;
using UnityEngine;

namespace TyrDK
{
    public struct TyrViewConfig
    {
        public Sprite AppIcon;
        public string AppName;
        public string AppGenre;
        public string AppDifficulty;
        public Sprite PointIcon;
        public int PointAmount;
        public string PointName;
        public int RwdAmount;
        
        public Sprite AppLargeImage;
        
        public string ClickUrl;
        
        public List<TyrTaskViewConfig> PlaytimeTaskViews;
        public List<TyrTaskViewConfig> PayoutTaskViews;
        public List<TyrMicroChargeEventViewConfig> MicroChargeEventViews;

        public void SortPlaytimeTaskViews()
        {
            PlaytimeTaskViews.Sort(SortById);
        }
        
        public void SortPayoutTaskViews()
        {
            PayoutTaskViews.Sort(SortById);
        }
        public int SortById(TyrTaskViewConfig a, TyrTaskViewConfig b)
        {
            return a.Id.CompareTo(b.Id);
        }
    }
    
    public struct TyrTaskViewConfig
    {
        public int Id;
        public string MainText;
        public string PointText;
        public string CompleteText;
        public bool IsActive;
        public float Progress;
        public Sprite PointIcon;
        public string EventCategory;
    }
    
    public struct TyrMicroChargeEventViewConfig
    {
        public string MainText;
        public int? TimeLimit;
        public int PointAmount;
        public Sprite PointIcon;
        public float ProgressAmount;
        public string ProgressAmountText;
        public string ProgressText;
        public int? BonusAmount;
    }
}