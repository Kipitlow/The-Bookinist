using System.Collections.Generic;
using UnityEngine;

public class CycleThroughSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool _cycle;

    private int _numberOfSprite;
    public int _currentFrame { get; private set; } = 0;
    public void Cycle(List<Sprite> sprites, bool cycle)
    {
        if (_currentFrame == 0)
        {
            _numberOfSprite = sprites.Count;
            _cycle = cycle;
        }


        if (_currentFrame < _numberOfSprite)
        {
            if (_cycle) _currentFrame = 0; 
            else _currentFrame = _numberOfSprite;
        }

        _currentFrame++;
        _spriteRenderer.sprite = sprites[_currentFrame];
    }

    public bool IsAtThisFrame(int wantedFrame, bool trueIfMore)
    {
        if (_currentFrame == wantedFrame) return true;
        if (trueIfMore && _currentFrame > wantedFrame) return true;
        return false;
    }
}
