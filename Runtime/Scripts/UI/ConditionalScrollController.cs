using System;
using UnityEngine;
using UnityEngine.UI;

public class ConditionalScrollController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform upperElement;
    [SerializeField] private RectTransform lowerElement;

    private float upperY, lowerX;
    private float minX, maxX, minY, maxY;
    private RectTransform content;

    void Start()
    {
        if (!scrollRect)
            scrollRect = GetComponent<ScrollRect>();
        upperY = upperElement.anchoredPosition.y;
        lowerX = lowerElement.anchoredPosition.x;

        var widthUpper = upperElement.rect.width;
        var widthLower = lowerElement.rect.width;
        var heightUpper = upperElement.rect.height;
        var heightLower = lowerElement.rect.height;
        content = scrollRect.content;
        minX = Mathf.Min(upperElement.anchoredPosition.x - widthUpper/2f, lowerElement.anchoredPosition.x - widthLower/2f);
        maxX = Mathf.Max(upperElement.anchoredPosition.x + widthUpper/2f, lowerElement.anchoredPosition.x + widthLower/2f);
        minY = Mathf.Min(upperElement.anchoredPosition.y - heightUpper/2f, lowerElement.anchoredPosition.y - heightLower/2f);
        maxY = Mathf.Max(upperElement.anchoredPosition.y + heightUpper/2f, lowerElement.anchoredPosition.y + heightLower/2f);
    }
    

    private void Update()
    {
        var clampedPos = content.anchoredPosition;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        content.anchoredPosition = clampedPos;
        var pos = scrollRect.content.anchoredPosition;
        if (Mathf.Approximately(pos.x, lowerX))
        {
            scrollRect.vertical = true;
            scrollRect.horizontal = true;
        }
        else if (pos.x < lowerX && pos.y > upperY)
        {
            scrollRect.vertical = false;
            scrollRect.horizontal = true;
        }
        else if (pos.x > lowerX && pos.y > upperY)
        {
            scrollRect.vertical = true;
            scrollRect.horizontal = false;
        }
        else if (pos.x > lowerX && pos.y < upperY)
        {
            content.anchoredPosition = clampedPos;
        }
        else if (pos.x < lowerX && pos.y > upperY)
        {
            scrollRect.vertical = true;
            scrollRect.horizontal = false;
        }
    }
}