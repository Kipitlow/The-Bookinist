using System.Collections;
using UnityEngine;

public class Script : MonoBehaviour
{
    #region Variable
    [Header("3Cercle")]
    [SerializeField] private GameObject _oneCercle;
    [SerializeField] private GameObject _twoCercle;
    [SerializeField] private GameObject _treeCercle;

    [Header("3Coroutine")]
    [SerializeField] private float _second1Coroutine;
    [SerializeField] private float _second2Coroutine;
    [SerializeField] private float _second3Coroutine;

    [Header("3Script ScriptSelfRoulette")]
    public ScriptSelfRoulette C2D_01;
    public ScriptSelfRoulette C2D_02;
    public ScriptSelfRoulette C2D_03;

    [Header("3Bool Coroutine")]
    private bool _boolOneCoroutine = false;
    private bool _boolTwoCoroutine = false;
    private bool _boolThreeCoroutine = false;

    [Header("Fonctionnement Coroutine")]
    private bool _enigmeRouletteTerminer = false;
    [SerializeField] private GameObject _prefable;
    [SerializeField] private Transform _targetSlotSpawn;

    /*
    [Header("Collision")]
    public UnityEvent<Collision> onCollisionEnterEvent;
    */

    #endregion
    #region Unity Methods
    private void Start()
    {
        intervertirCouroutine(GameObject.Find("FBX_1erRoueCerber"));
        intervertirCouroutine(GameObject.Find("FBX_2emeRoueCerber"));
        intervertirCouroutine(GameObject.Find("FBX_3emeRoueCerber"));
    }
    #endregion
    #region Methods
    public void intervertirCouroutine(GameObject other)
    {
        if(_enigmeRouletteTerminer == false)
        {
            switch (other.name)
            {
                case "FBX_1erRoueCerber":
                    if(_boolOneCoroutine == false)
                    {
                        if (C2D_01.DetectCollision != true)
                        {
                            _boolOneCoroutine = true;
                            StartCoroutine(_premierCercle());
                        }                        
                    }
                    else
                    {
                        _boolOneCoroutine = false;
                        StopCoroutine(_premierCercle());
                    }
                    if (C2D_01.DetectCollision == true) _oneCercle.GetComponent<Renderer>().material.color = new Color(90.0f / 255.0f, 90.0f / 255.0f, 90.0f / 255.0f, 255.0f);
                    break;
                case "FBX_2emeRoueCerber":
                    if (_boolTwoCoroutine == false)
                    {
                        if (C2D_02.DetectCollision != true)
                        {
                            _boolTwoCoroutine = true;
                            StartCoroutine(_deuxiémeCercle());
                        }
                    }
                    else
                    {
                        _boolTwoCoroutine = false;
                        StopCoroutine(_deuxiémeCercle());
                    }
                    if (C2D_02.DetectCollision == true) _twoCercle.GetComponent<Renderer>().material.color = new Color(90.0f / 255.0f, 90.0f / 255.0f, 90.0f / 255.0f, 255.0f);
                    break;
                case "FBX_3emeRoueCerber":
                    if (_boolThreeCoroutine == false)
                    {
                        if (C2D_03.DetectCollision != true)
                        {
                            _boolThreeCoroutine = true;
                            StartCoroutine(_troisiémeCercle());
                        }
                    }
                    else
                    {
                        _boolThreeCoroutine = false;
                        StopCoroutine(_troisiémeCercle());
                    }
                    if (C2D_03.DetectCollision == true) _treeCercle.GetComponent<Renderer>().material.color = new Color(90.0f / 255.0f, 90.0f / 255.0f, 90.0f / 255.0f, 255.0f);
                    break;
            }

            if (C2D_01.DetectCollision && _boolOneCoroutine == false && C2D_02.DetectCollision && _boolTwoCoroutine==false && C2D_03.DetectCollision && _boolThreeCoroutine == false)
            {
                _enigmeRouletteTerminer = true;

                GameObject instancierGameObject = Instantiate(_prefable, _targetSlotSpawn.transform.position, this.transform.rotation);
                //instancierGameObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
    }
    IEnumerator _premierCercle()
    {
        while (_boolOneCoroutine)
        {
            _oneCercle.transform.Rotate(0.0f, _second1Coroutine * Time.deltaTime, 0.0f);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator _deuxiémeCercle()
    {
        while (_boolTwoCoroutine)
        {
            _twoCercle.transform.Rotate(0.0f,_second2Coroutine * Time.deltaTime, 0.0f);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator _troisiémeCercle()
    {
        while (_boolThreeCoroutine)
        {
            _treeCercle.transform.Rotate(0.0f, _second3Coroutine * Time.deltaTime, 0.0f);
            yield return new WaitForSeconds(0.01f);
        }
    }
    #endregion
}
