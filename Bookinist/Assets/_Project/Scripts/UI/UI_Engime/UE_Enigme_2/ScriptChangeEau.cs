using System.Collections;
using UnityEngine;

public class ScriptChangeEau : MonoBehaviour
{
    private InteractionRunner _scriptActivation;
    private BoxCollider boxCollider;
    private SpriteRenderer _selfSprite;
    private int _nombreNuage;
    private float _lerpColorFloat;
    private bool _finishEnigmeEtang = false;
    void Start()
    {
        _selfSprite = GetComponent<SpriteRenderer>();
        _scriptActivation = GetComponent<InteractionRunner>();
        boxCollider= GetComponent<BoxCollider>();
        Invoke("_DisableInteractionRunner", 0.1f);
        _nombreNuage = 0;
        _lerpColorFloat = 0;
    }

    private void _DisableInteractionRunner()
    {
        _scriptActivation.enabled = false;
        boxCollider.enabled = false;
    }

    public void addIntCloud()
    {
        _nombreNuage += 1;
        if (_nombreNuage >= 3)
        {
            StartCoroutine(IChangeColor());
        }
    }

    private IEnumerator IChangeColor()
    {
        //new Color(1, 1, 0, 1); => this at the color is red
        //new Color(0, 0, 1, 1); => this at the color is red
        _lerpColorFloat = 0;

        Color _colorOrigine = new Color(140.0f/ 255.0f,166.0f/255.0f, 163.0f / 255.0f, 1f);
        Color _colorChange = new Color(230.0f / 255.0f, 255.0f / 255.0f, 168.0f/255.0f, 1f);
        while (_lerpColorFloat < 1)
        {
            _lerpColorFloat+= 0.1f;
            //_selfSprite.color = Color.Lerp(Color.white, Color.yellow, _lerpColorFloat);
            _selfSprite.color = Color.Lerp(_colorOrigine, _colorChange, _lerpColorFloat);
            yield return new WaitForSeconds(0.1f);
        }
        _scriptActivation.enabled= true;
        boxCollider.enabled= true;
    }

    public void starCoroutineChangeColor()
    {
        Debug.Log("start couroutine");
        if (!_finishEnigmeEtang)
        {
            StartCoroutine(IChangeColor());
            _finishEnigmeEtang = true;
        }
    }
}
