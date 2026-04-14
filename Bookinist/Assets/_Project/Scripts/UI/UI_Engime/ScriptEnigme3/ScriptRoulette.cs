using System.Collections;
using UnityEngine;

public class Script : MonoBehaviour
{
    [SerializeField] private GameObject _oneCercle;
    [SerializeField] private GameObject _twoCercle;
    [SerializeField] private GameObject _treeCercle;

    [SerializeField] private float _second1Coroutine;
    [SerializeField] private float _second2Coroutine;
    [SerializeField] private float _second3Coroutine;

    private bool _boolOneCoroutine;
    private bool _boolTwoCoroutine;
    private bool _boolThreeCoroutine;
    private void Start()
    {
        StartCoroutine(_premierCercle());
        StartCoroutine(_deuxiémeCercle());
        StartCoroutine(_troisiémeCercle());
    }

    public void StartcouroutineRotation(int RR)
    {
        switch(RR)
        {
            case 0:
                StartCoroutine(_premierCercle());
                _boolOneCoroutine = false;
                if (_boolOneCoroutine == true)
                {
                    
                }
                else
                {
                    StopCoroutine(_premierCercle());
                    _boolOneCoroutine = true;
                }
                    break;
            case 1:
                if (_boolTwoCoroutine == true)
                {
                    StartCoroutine(_deuxiémeCercle());
                    _boolTwoCoroutine = false;
                }
                else
                {
                    StopCoroutine(_deuxiémeCercle());
                    _boolTwoCoroutine = true;
                }
                break;
            case 2:
                if (_boolThreeCoroutine == true)
                {
                    StartCoroutine(_troisiémeCercle());
                }
                else
                {
                    StopCoroutine(_troisiémeCercle());
                }
                break;
        }
    }
    public void StopcouroutineRotation(int RR)
    {
        switch(RR)
        {
            case 0:
                StopCoroutine(_premierCercle());
                _boolOneCoroutine = true;
                break;
            case 1:
                StopCoroutine(_deuxiémeCercle());
                _boolTwoCoroutine = true;
                break;
            case 2:
                if (_boolThreeCoroutine == true)
                {
                    StartCoroutine(_troisiémeCercle());
                }
                else
                {
                    StopCoroutine(_troisiémeCercle());
                }
                break;
        }
    }
    IEnumerator _premierCercle()
    {
        while (_boolOneCoroutine)
        {
            _oneCercle.transform.Rotate(0.0f, 0.0f, _second1Coroutine * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator _deuxiémeCercle()
    {
        while (_boolTwoCoroutine)
        {
            _twoCercle.transform.Rotate(0.0f, 0.0f, _second2Coroutine * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator _troisiémeCercle()
    {
        while (_boolThreeCoroutine)
        {
            _treeCercle.transform.Rotate(0.0f, 0.0f, _second3Coroutine * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
