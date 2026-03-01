using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    [Range(3.0f, 20.0f)]
    [SerializeField] private float _duration = 5.0f;
    [Range(0.0f, 20.0f)]
    [SerializeField] private float _startingDelay = 0.0f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        
        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        if (_startingDelay > 0f)
            yield return new WaitForSeconds(_startingDelay);

        float elapsedTime = 0f;

        if (_button != null)
            _button.interactable = true;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / _duration);
            _image.color = color;
            yield return null;
        }

        color.a = 1f;
        _image.color = color;
    }

}
