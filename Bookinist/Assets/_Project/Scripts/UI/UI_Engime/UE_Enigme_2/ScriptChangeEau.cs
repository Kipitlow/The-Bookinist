using System.Collections;
using UnityEngine;

public class ScriptChangeEau : MonoBehaviour
{
    #region Variable
    private float _lerpColorFloat=0;
    private Material _material;
    private InteractionRunner _interactionRunner;
    private bool _hasChangedColor = false;
    #endregion
    #region Methode Variable
    void Start()
    {
        _material = GetComponentInChildren<SpriteRenderer>().material;
        _interactionRunner = GetComponentInChildren<InteractionRunner>();

        Invoke("_DisableInteractionRunner", 0.1f);
        _material.SetColor("BaseColor", Color.green);
    }
    #endregion

    private void FixedUpdate()
    {
        if (!_hasChangedColor)
        {
            _interactionRunner.CallTry();
        }
    }

    public void ChangeColor()
    {
        StartCoroutine(IEChangeColor());
        _hasChangedColor = true;
    }
    public void printStatut()
    {
        Debug.Log("Tourne Banddéla");
    }
    private IEnumerator IEChangeColor()
    {
        _lerpColorFloat = 0;

        Color _colorOrigine = new Color(46.0f / 255.0f, 123.0f / 255.0f, 197.0f / 255.0f, 1f);
        Color _colorChange = new Color(255.0f / 255.0f, 170.0f / 255.0f, 103.0f / 255.0f, 1f);
        Color _changingShadering;
        while (_lerpColorFloat < 1)
        {
            _lerpColorFloat+= 0.1f;
            //_selfSprite.color = Color.Lerp(Color.white, Color.yellow, _lerpColorFloat);
            //_selfSprite.color = Color.Lerp(_colorOrigine, _colorChange, _lerpColorFloat);
            _changingShadering = Color.Lerp(_colorOrigine, _colorChange, _lerpColorFloat);
            if(_material!=null)_material.SetColor("BaseColor", _changingShadering);
            if(_material!=null)_material.SetColor("_BaseColor", _changingShadering);
            Debug.Log("<color=green> Success</color>" + "ColorMaterial: " + _changingShadering);
            yield return new WaitForSeconds(0.1f);
        }
        //_scriptActivation.enabled= true;
        //_boxCollider.enabled= true;
    }
}