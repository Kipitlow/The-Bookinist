using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BurnLetter : MonoBehaviour
{
    [SerializeField] Image _image;
    IEnumerator _appearRoutine;
    IEnumerator _disappearRoutine;
    float _appearAlpha = -1000f;
    float _disappearAlpha = 0f;
    [SerializeField] float _durationTimer;
    void Start()
    {
        _image = GetComponent<Image>();
        _image.material.SetFloat("_Cutoff_Height", -1000f);
    }
    public void BeginDissolve(int secondsWait)
    {
        StopAllCoroutines();
        _disappearRoutine = null;
        IEnumerator Routine(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            var startTime = Time.time;
            var duration = _durationTimer;
            while (Time.time - startTime < duration)
            {
                Debug.Log("Camouflage On");
                _disappearAlpha = Mathf.Lerp(-1000f, 1200f, (Time.time - startTime) / duration);
                _image.material.SetFloat("_Cutoff_Height", _disappearAlpha);
                yield return null;
            }
        }
        _appearRoutine = Routine(secondsWait);
        StartCoroutine(_appearRoutine);
    }

    public void StopDissolve()
    {
        StopAllCoroutines();
        _appearRoutine = null;

        IEnumerator Routine()
        {
            var startTime = Time.time;
            var duration = _durationTimer;

            while (Time.time - startTime < duration)
            {
                Debug.Log("Camouflage Off");
                _appearAlpha = Mathf.Lerp(1200f, -1000f, (Time.time - startTime) / duration);
                _image.material.SetFloat("_Cutoff_Height", _appearAlpha);
                yield return null;
            }
        }
        _disappearRoutine = Routine();
        StartCoroutine(_disappearRoutine);
    }


}