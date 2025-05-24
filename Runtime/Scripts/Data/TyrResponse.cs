using System;
using System.Collections.Generic;

namespace TyrDK
{
    [Serializable]
    public class TyrCampaigns
    {
        public List<CampaignData> data;
        public string message;
    }
    
    [Serializable]
    public class TyrCampaignDetails
    {
        public CampaignData data;
        public string message;
    }
    [Serializable]
    public class CampaignData
    {
        public int campaignId;
        public string campaignName;
        public string campaignDescription;
        public string createdOn;
        public int sortingScore;
        public string status;
        public string expiredOn;
        public bool hasPlaytimeEvents;
        public bool capReached;
        public int premium;
        public App app;
        public Currency currency;
        public CampaignPayout campaignPayout;
        public Tracking tracking;
        public Targeting targeting;
        public Creative creative;
        public bool isInstalled;
        public bool isRetryDownload;
        public List<PayoutEvent> payoutEvents;
        public MicroCharge microCharge;
        public List<MicroChargeEvent> microChargeEvents;
        public List<PlaytimeEvent> playtimeEvents;
        public EarnedPayout earnedPayout;
    }

    [Serializable]
    public class App
    {
        public int id;
        public string title;
        public string packageName;
        public double rating;
        public string shortDescription;
        public string store;
        public string storeCategory;
        public string previewUrl;
        public string thumbnail;
    }

    [Serializable]
    public class Currency
    {
        public string name;
        public string symbol;
        public string adUnitName;
        public string adUnitCurrencyName;
        public int adUnitCurrencyConversion;
        public string adUnitCurrencyIcon;
    }

    [Serializable]
    public class CampaignPayout
    {
        public int totalEvents;
        public int totalPayout;
        public float totalPayoutConverted;
        public float totalPlayablePayout;
        public float totalMicrochargePayout;
        public float totalPlayablePayoutConverted;
        public float totalMicrochargePayoutConverted;
    }

    [Serializable]
    public class Tracking
    {
        public string impressionUrl;
        public string clickUrl;
        public string s2sClickUrl;
    }

    [Serializable]
    public class Targeting
    {
        public string os;
        public string targetingType;
        public Reward reward;
    }

    [Serializable]
    public class Reward
    {
        public string rewardDifficulty;
        public string incentRewardDescription;
    }

    [Serializable]
    public class Creative
    {
        public string creativeUrl;
        public List<CreativePack> creativePacks;
    }

    [Serializable]
    public class CreativePack
    {
        public string creativePackName;
        public string languageName;
        public string languageCode;
        public List<CreativeItem> creatives;
    }

    [Serializable]
    public class CreativeItem
    {
        public string creativeName;
        public string callToAction;
        public string text;
        public string byteSize;
        public string fileUrl;
        public string duration;
        public CreativeType creativeType;
    }

    [Serializable]
    public class CreativeType
    {
        public string name;
        public string type;
        public string width;
        public string height;
        public string creativeCategoryType;
    }

    [Serializable]
    public class PayoutEvent
    {
        public int id;
        public string conversionStatus;
        public string identifier;
        public string eventName;
        public string eventDescription;
        public string eventCategory;
        public int payoutAmount;
        public int payoutAmountConverted;
        public int payoutTypeId;
        public string payoutType;
        public bool allowDuplicateEvents;
        public int? maxTime;
        public string maxTimeMetric;
        public int? maxTimeRemainSeconds;
        public bool enforceMaxTimeCompletion;
    }

    [Serializable]
    public class MicroCharge
    {
        public float earned;
        public float earnedConversion;
        public float total;
        public float totalConversion;
    }

    [Serializable]
    public class MicroChargeEvent : PayoutEvent
    {
        public int dailyCount;
        public int? dailyLimit;
        public int count;
        public int? limit;
    }

    [Serializable]
    public class PlaytimeEvent
    {
        public int id;
        public string conversionStatus;
        public int payoutAmount;
        public int payoutAmountConverted;
        public int timePlayedSeconds;
    }

    [Serializable]
    public class EarnedPayout
    {
        public float earnedPlayablePayout;
        public float earnedMicrochargePayout;
        public float earnedPlaytimePayout;
        public float earnedPlayablePayoutConverted;
        public float earnedMicrochargePayoutConverted;
        public float earnedPlaytimePayoutConverted;
    }
}