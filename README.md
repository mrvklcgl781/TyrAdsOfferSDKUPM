# TyrAds SDK for Unity

This package provides integration with the TyrAds SDK for Unity projects.

## Installation

1. Open your Unity project
2. Open the Package Manager (Window > Package Manager)
3. Click the + button in the top-left corner
4. Select "Add package from disk..."
5. Navigate to and select the `package.json` file in the current directory

## Requirements

- Unity 2022.3 or later
- TextMesh Pro 3.0.6 or later

## Usage

### Initialization

To start using the SDK, you need to initialize it first. The best place to do this is in your game's initialization code:

```csharp
using TyrDK;

// Initialize the SDK
TyrOfferSDK.Instance.InitializeSDK();
```

### Showing Offer Wall

The SDK provides two main methods to display offer walls:

1. Show the main offer wall with a list of campaigns:
```csharp
TyrOfferSDK.Instance.ShowOfferWall((campaigns) => {
    // Handle the received campaigns
    foreach (var campaign in campaigns) {
        Debug.Log($"Campaign ID: {campaign.campaignId}");
    }
});
```

2. Show details for a specific campaign:
```csharp
// Replace campaignId with the actual ID of the campaign
int campaignId = 123;
TyrOfferSDK.Instance.ShowOfferWallDetails(campaignId);
```

### Example Implementation

Here's a complete example of how to implement the SDK in your game:

```csharp
using TyrDK;
using UnityEngine;

public class YourGameManager : MonoBehaviour
{
    void Start()
    {
        // Initialize the SDK
        TyrOfferSDK.Instance.InitializeSDK();
        
        // Show the offer wall
        TyrOfferSDK.Instance.ShowOfferWall(OnCampaignsReceived);
    }

    private void OnCampaignsReceived(List<CampaignData> campaigns)
    {
        // Handle the received campaigns
        foreach (var campaign in campaigns)
        {
            // Process each campaign
            Debug.Log($"Received campaign: {campaign.campaignId}");
        }
    }

    // Call this method when you want to show details for a specific campaign
    public void ShowCampaignDetails(int campaignId)
    {
        TyrOfferSDK.Instance.ShowOfferWallDetails(campaignId);
    }
}
```

### Best Practices

1. Initialize the SDK early in your game's lifecycle
2. Handle the campaign data appropriately in the callback
3. Make sure to test the offer wall in both development and production environments
4. Consider implementing error handling for network-related issues

## Support

For support, please contact support@tyrads.com or visit our website at https://www.tyrads.com