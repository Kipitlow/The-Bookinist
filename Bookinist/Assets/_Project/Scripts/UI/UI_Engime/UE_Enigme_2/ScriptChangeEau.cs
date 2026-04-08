using System.Collections;
using UnityEngine;

public class ScriptChangeEau : MonoBehaviour
{
    private InteractionRunner _scriptActivation;
    private SpriteRenderer _selfSprite;
    private int _nombreNuage;
    private float _lerpColorFloat;
    void Start()
    {
        _selfSprite = GetComponent<SpriteRenderer>();
        _scriptActivation = GetComponent<InteractionRunner>();
        Invoke("_DisableInteractionRunner", 0.1f);
        _nombreNuage = 0;
        _lerpColorFloat = 0;
    }

    private void _DisableInteractionRunner()
    {
        _scriptActivation.enabled = false;
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
        while (_lerpColorFloat < 1)
        {
            _lerpColorFloat+= 0.1f;
            _selfSprite.color = Color.Lerp(Color.blue, Color.yellow, _lerpColorFloat);
            yield return new WaitForSeconds(0.1f);
        }
        _scriptActivation.enabled= true;
    }
}
