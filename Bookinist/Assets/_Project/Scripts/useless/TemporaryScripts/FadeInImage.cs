using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI[] _texts;
    [SerializeField] private Button _button;

    [Header("Timing")]
    [SerializeField, Range(3f, 20f)] private float _duration = 5f;
    [SerializeField, Range(0f, 20f)] private float _startingDelay = 0f;

    private bool _isFading = true;

    private void OnEnable()
    {
        Debug.Log(_isFading);
        if (_isFading)
        {
            _isFading = false;
            return;
        }

        FullyActivateButton();

        FadeImage();
        //FadeText();

    }

    private void FullyActivateButton()
    {
        //if (_image != null)
        //{
        //    Color color = _image.color;
        //    color.a = 1f;
        //    _image.color = color;
        //}

        //if (_button != null)
        //    _button.interactable = true;
    }

    private void Start()
    {
        //bool firstCustomerEncounter = GameManager.Instance._isFirstCustomerEncounter;

        //if (_image != null)
        //    StartCoroutine(FadeImage());

        //if (_texts == null || _texts.Length == 0)
        //    return;

        //int textIndex = firstCustomerEncounter ? 1 : 0;

        //if (firstCustomerEncounter && _texts.Length > 0)
        //    _texts[0].gameObject.SetActive(false);

        //StartCoroutine(FadeText(_texts[textIndex]));
    }

    private IEnumerator FadeImage()
    {
        yield return new WaitForSeconds(_startingDelay);

        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / _duration);
            _image.color = color;
            yield return null;
        }

        color.a = 1f;
        _image.color = color;

        if (_button != null)
            _button.interactable = true;
    }

    private IEnumerator FadeText(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(_startingDelay);

        if (_button != null)
            _button.interactable = true;

        text.gameObject.SetActive(true);

        Color color = text.color;
        color.a = 0f;
        text.color = color;

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / _duration);
            text.color = color;
            yield return null;
        }

        color.a = 1f;
        text.color = color;
    }
}