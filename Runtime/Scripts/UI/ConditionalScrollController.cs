using System;
using UnityEngine;
using UnityEngine.UI;

public class ConditionalScrollController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform upperElement;
    [SerializeField] private RectTransform lowerElement;

    private float _upperY, _lowerX;
    private RectTransform _content;
    private bool _isInitialized = false;
    private Action _onUpdate;
    private bool _wasHorizontalScrolling = false;
    private Vector2 _lastContentPosition;
    public event Action OnHorizontalScroll, OnVerticalScroll;
    public RectTransform Content
    {
        get
        {
            if (!_content)
            {
                if (!scrollRect)
                    scrollRect = GetComponent<ScrollRect>();
                _content = scrollRect.content;
            }
            return _content;
        }
    }
    
    public Vector2 GetContentAreaTopLeftPosition()
    {
        if (!scrollRect)
            scrollRect = GetComponent<ScrollRect>();
        _content = scrollRect.content;
        return new Vector2(_content.anchoredPosition.x - _content.rect.width / 2, _content.anchoredPosition.y + _content.rect.height / 2);
    }
    
    public void SetConditional(Vector2 size)
    {
        if (!scrollRect)
            scrollRect = GetComponent<ScrollRect>();
        
        _content.sizeDelta = size;
        _upperY = _content.transform.position.y;
        _lowerX = _content.transform.position.x;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        
        _isInitialized = true;
        _onUpdate = OnConditionalUpdate;
        _lastContentPosition = _content.anchoredPosition;
    }
    
    
    public void SetRegularHorizontalScroll(Vector2 position,Vector2 size)
    {
        if (!scrollRect)
            scrollRect = GetComponent<ScrollRect>();
            
        _content = scrollRect.content;
        _content.sizeDelta = size;
        _content.anchoredPosition = position;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        scrollRect.horizontal = true;
        scrollRect.vertical = false;
        _isInitialized = true;
        _lastContentPosition = position;
    }

    private void OnConditionalUpdate()
    {
        if (!_isInitialized) return;
        var worldPos = _content.transform.position;
        if (worldPos.x >= _lowerX)
        {
            scrollRect.vertical = true;
            scrollRect.horizontal = true;
        }
        else if(worldPos.y < _upperY)
        {
            // In between, only vertical
            scrollRect.vertical = true;
            scrollRect.horizontal = false;
            _wasHorizontalScrolling = false;
        }
        if (_isInitialized && _content != null)
        {
            DetectHorizontalScrolling();
        }
    }

    private void Update()
    {
        _onUpdate?.Invoke();
    }
    private void DetectHorizontalScrolling()
    {
        Vector2 currentPosition = _content.anchoredPosition;
        if (Mathf.Abs(currentPosition.x - _lastContentPosition.x) > 0.1f)
        {
            if (!_wasHorizontalScrolling)
            {
                _wasHorizontalScrolling = true;
                OnHorizontalScroll?.Invoke();
            }
        }
        else
        {
            _wasHorizontalScrolling = false;
            OnVerticalScroll?.Invoke();
        }
        _lastContentPosition = currentPosition;
    }
}