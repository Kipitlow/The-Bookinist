using System.Collections;
using UnityEngine;

public class ScriptChangeEau : MonoBehaviour
{
    private InteractionRunner _scriptActivation;
    private BoxCollider boxCollider;
    private SpriteRenderer _selfSprite;
    private int _nombreNuage;
    private float _lerpColorFloat;

    void Start()
    {
        _selfSprite = GetComponent<SpriteRenderer>();
        _scriptActivation = GetComponent<InteractionRunner>();
        boxCollider= GetComponent<BoxCollider>();
        Invoke("_DisableInteractionRunner", 0.1f);
        _nombreNuage = 0;
        _lerpColorFloat = 0;

    }

    /*private void _DisableInteractionRunner()
    {
        _scriptActivation.enabled = false;
        boxCollider.enabled = false;
    }*/

    public void ChangeColor()
    {
        StartCoroutine(IEChangeColor());
    }

    private IEnumerator IEChangeColor()
    {
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
}
