using System.Collections;
using UnityEngine;

public class ScriptMoneyShop : MonoBehaviour
{
    #region Variable
    [Header("VariablePiece")]
    public RectTransform targetTransform;
    private float _currentLerpPosition = 0;
    private float _impulsefloat;

    [Header("TypeMoney")]
    private string _typeMoney;
    private int _currentMoney;

    #endregion
    #region Method Unity
    public void play(int _current, string _name)
    {
        _typeMoney = _name;
        _currentMoney = _current;
        _addImpulse();
        Invoke("startMoving", 2.0f);
    }
    #endregion
    #region Unity
    private void startMoving()
    {
        StartCoroutine(MovePiece());
    }
    private void _addImpulse()
    {
        Rigidbody2D _rb2 = gameObject.GetComponent<Rigidbody2D>();
        float _randomfloat = Random.Range(-25f,25);
        _impulsefloat = Random.Range(2, 5);
        Vector2 _direction = new Vector2(_randomfloat, 1.0f);
        _rb2.AddForce(_direction * _impulsefloat, ForceMode2D.Impulse);
        StartCoroutine(_influenceGraviter());
    }
    IEnumerator _influenceGraviter()
    {
        Rigidbody2D _rb2 = gameObject.GetComponent<Rigidbody2D>();
        _rb2.gravityScale = 1.0f;
        while (_rb2.gravityScale > 0)
        {
            _rb2.gravityScale -= 5f;
            Debug.Log($"Gravity: {_rb2.gravityScale}");
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator MovePiece()
    {
        #region MoveMoney
        _currentLerpPosition = 0;
        Rigidbody2D _rb2 = gameObject.GetComponent<Rigidbody2D>();
        _rb2.simulated = false;
        while (_currentLerpPosition<0.1)
        {
            _currentLerpPosition += 0.001f;
            Vector2 _lerpPosition = Vector2.Lerp(this.transform.position, targetTransform.position, _currentLerpPosition);
            transform.localRotation = Quaternion.Lerp(this.transform.localRotation, targetTransform.localRotation, _currentLerpPosition);
            Vector2 _lerpScale = Vector2.Lerp(this.transform.localScale, Vector2.zero, _currentLerpPosition);
            transform.position = _lerpPosition; transform.localScale = _lerpScale;

            yield return new WaitForSeconds(0.01f);
        } //Move items
        #endregion
        #region AddMoney
        CurrencyManager _currencyManager = GameObject.FindAnyObjectByType<CurrencyManager>();
        if( _currencyManager != null )
        {
            switch (_typeMoney)
            {
                case "Franc":
                    _currencyManager.AddSoftCurrency(_currentMoney);
                    break;
                case "Gemme":
                    _currencyManager.AddHardCurrency(_currentMoney);
                    break;
                case "Plume":
                    //_currencyManager.AddHardCurrency(_currentMoney);
                    Debug.LogWarning("Crow");
                    break;
            }
        }
        else
        {
            Debug.LogError("!!! No Resulte CurrentManager !!!");
        }
        #endregion
        #region ChangeScaleEndTransparent
        SpriteRenderer _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        float _alpha=1;
        while (_alpha > 0)//tranparent
        {
            _alpha -= 0.05f;
            _spriteRenderer.color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, _alpha);
            yield return new WaitForSeconds(0.05f);
        }
        #endregion
    }
    #endregion
}
