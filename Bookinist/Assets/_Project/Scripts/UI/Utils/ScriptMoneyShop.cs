using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ScriptMoneyShop : MonoBehaviour
{
    #region Variable
    [Header("VariablePiece")]
    public RectTransform targetTransform;
    private float _impulsefloat;
    private float _currentLerpPosition = 0;
    #endregion
    #region Method Unity
    private void Start()
    {
        _addImpulse();
        Invoke("startMoving", 2.0f);
        //StartCoroutine(MovePiece());
    }
    #endregion
    #region Unity
    public void startMoving()
    {
        StartCoroutine(MovePiece());
    }

    private void _addImpulse()
    {
        Rigidbody2D _rb2 = gameObject.GetComponent<Rigidbody2D>();
        
        float _randomfloat = Random.Range(-0.2f,0.2f);
        _impulsefloat = Random.Range(600.0f, 1500.0f);
        Vector2 _direction = new Vector2(_randomfloat, 1.0f);
        _rb2.AddForce(_direction * _impulsefloat, ForceMode2D.Impulse);
        StartCoroutine(_influenceGraviter());
    }
    IEnumerator _influenceGraviter()
    {
        Rigidbody2D _rb2 = gameObject.GetComponent<Rigidbody2D>();
        while (_rb2.gravityScale > 0)
        {
            _rb2.gravityScale -= 5f;
            Debug.Log($"Gravity: {_rb2.gravityScale}");
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator MovePiece()
    {
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
        }
        Debug.Log("<color=green>[Succes: Déplacement]</color>");
        SpriteRenderer _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        float _alpha=1;
        while (_alpha > 0)
        {
            _alpha -= 0.05f;
            _spriteRenderer.color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, _alpha);
            yield return new WaitForSeconds(0.05f);
        }

    }
    #endregion
}
