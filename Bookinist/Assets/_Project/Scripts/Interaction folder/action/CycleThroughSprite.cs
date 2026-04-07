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

        _currentFrame++;

        if (_currentFrame > _numberOfSprite - 1)
        {
            if (_cycle) _currentFrame = 0; 
            else _currentFrame = _numberOfSprite - 1;
        }
        print("current frame : " + _currentFrame + " number of sprites : " + _numberOfSprite);
        Debug.Log("current frame : " + _currentFrame + " number of sprites : " + _numberOfSprite);
        _spriteRenderer.sprite = sprites[_currentFrame];
    }

    public bool IsAtThisFrame(int wantedFrame, bool trueIfMore)
    {
        if (_currentFrame == wantedFrame) return true;
        if (trueIfMore && _currentFrame > wantedFrame) return true;
        return false;
    }
}
