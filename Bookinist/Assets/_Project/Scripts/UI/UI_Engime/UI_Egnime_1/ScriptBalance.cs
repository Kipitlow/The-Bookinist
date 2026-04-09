using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScriptBalance : MonoBehaviour
{
    #region Variable
    [Header("Controller Rotation")]
    //[SerializeField] public GameObject UI_Gameplay;
    public GameObject balanceSprite;
    public GameObject balance;
    public GameObject plateau1;
    public GameObject plateau2;

    [Header("Gestion UI")]
    //[SerializeField] public GameObject UI_Gameplay;
    public GameObject Prefable_Poids;

    [Header("Autre")]
    [SerializeField] public GameObject[] Button_Hidden;
    private GameObject Poids;
    private GameObject Poids2;
    private bool _targetOne;
    private int _poidAccumuler=0;

    #endregion

    void Start()
    {
        _targetOne=true;    
    }

    private void Update()
    {
        if (balance == null) Debug.LogError("Balance éclater au sol");
        Vector3 Euler = balance.transform.eulerAngles;

        balanceSprite.transform.localRotation = Quaternion.Euler(Euler.x, Euler.y, Euler.z);
        plateau1.transform.rotation = Quaternion.Euler(0, 0, 0);
        plateau2.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void FinishTask()
    {
        print("finish task");
        Invoke("CacheSelf", 2.0f);
    }
    private void CacheSelf()
    { 
        gameObject.SetActive(false);
    }
    public void SpawnerPoidsBalance(int Mass)
    {
        /*if (Poids != null)
        {
            Destroy(Poids);
            Poids=null;
        }
        if (Poids2 != null)
        {
            Destroy(Poids2);
            Poids2 = null;
        }*/


        Transform _ballMass = GameObject.Find("Target_spawn_Poids_R").transform;
        Poids = Instantiate(Prefable_Poids, _ballMass);

        if (Poids.GetComponent<Rigidbody>() != null) { Debug.LogError("Pourquoi ce foutus de ce RigBody"); }

        _ballMass = GameObject.Find("Target_spawn_Poids_L").transform;
        if (_targetOne==true)
        {
            _targetOne=false;   
            Poids2 = Instantiate(Prefable_Poids, _ballMass);
            Poids2.GetComponent<Rigidbody2D>().mass = 100;
        }
        switch (Mass)
        {
            case 0:
                if (Poids != null)
                {
                    Destroy(Poids);
                    Poids = null;
                }
                break;
            case 1:
                Poids.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                Poids.GetComponent<Rigidbody2D>().mass = 0;
                break;
            case 2:
                Poids.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
                Poids.GetComponent<Rigidbody2D>().mass = 100;
                break;
            case 3:
                Poids.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                Poids.GetComponent<Rigidbody2D>().mass = 25;
                break;
            case 4:
                Poids.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
                Poids.GetComponent<Rigidbody2D>().mass = 230;
                break;
            case 5:
                Poids.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                Poids.GetComponent<Rigidbody2D>().mass = 175;
                break;
        }


    }
    

}
