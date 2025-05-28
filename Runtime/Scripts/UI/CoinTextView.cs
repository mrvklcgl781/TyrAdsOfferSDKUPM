using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TyrDK
{
    public class CoinTextView : MonoBehaviour
    {
        [SerializeField] private Image coinIcon;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private float spacing = 10f;
        [SerializeField] private RectTransform rectTransform;
        
        public void SetView(Sprite icon, int amount, string currencyName)
        {
            if (coinIcon != null)
            {
                coinIcon.sprite = icon;
            }
            if (coinText != null)
            {
                coinText.text = FormatText(amount) + " " + currencyName;
            }

            SetIcon();
        }

        private void SetIcon()
        {
            if (coinIcon != null && coinText != null)
            {
                float textWidth = -coinText.preferredWidth - spacing;
                coinText.rectTransform.sizeDelta = new Vector2(coinText.preferredWidth, coinText.rectTransform.sizeDelta.y);
                coinIcon.rectTransform.anchoredPosition = new Vector2(textWidth, 0);
                rectTransform.sizeDelta = new Vector2(textWidth + coinIcon.rectTransform.sizeDelta.x,
                    rectTransform.sizeDelta.y);
            }
        }
        
        public static string FormatText(int amount)
        {
            if (amount >= 1000000)
            {
                amount /= 1000000;
                return amount + "M";
            }
            else if (amount >= 1000)
            {
                amount /= 1000;
                return amount +  "K";
            }
            else
            {
                return amount.ToString();
            }
        }
    }
}