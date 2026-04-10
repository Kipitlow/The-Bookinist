using System;
using System.Collections;
using System.Collections.Generic;
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
    public Item lyreSTP;

    [Header("Autre")]
    [SerializeField] public GameObject[] Button_Hidden;
    private GameObject Poids;
    private GameObject Poids2;
    private bool _targetOne; //permet de d'instantier le deuxiéme poids "c'est la lyre"
    private int _poidAccumuler=0;
    private List<Item> _stockIteam = new();

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
            case 100:
                Poids.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                Poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
            case 2:
                Poids.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
                Poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
            case 3:
                Poids.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                Poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
            case 4:
                Poids.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
                Poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
            case 5:
                Poids.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                Poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
        }


    }
    
    public void SpawnerIteamBalance(Item Context )
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
        //Poids = Instantiate(Prefable_Poids, _ballMass);
        Poids = Instantiate(Prefable_Poids, _ballMass);
        
        if (Poids.GetComponent<Rigidbody>() != null) { Debug.LogError("Pourquoi ce foutus de ce RigBody"); }

        _ballMass = GameObject.Find("Target_spawn_Poids_L").transform;
        if (_targetOne==true)
        {
            _targetOne=false;   
            Poids2 = Instantiate(Prefable_Poids, _ballMass);
            Poids2.GetComponent<Rigidbody2D>().mass = 100;
            Poids2.GetComponent<SpriteRenderer>().sprite = lyreSTP.itemSprite;
            Poids2.GetComponent<BoxCollider2D>().size = new Vector2(4.78f, 6f);
            Poids2.transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
            //X:4.78; Y:6
        }


        switch (Context.name)
        {
            case "":
                if (Poids != null)
                {
                    Destroy(Poids);
                    _poidAccumuler =0;
                    Poids = null;
                }
                break;
            case "Citron" or "Figue" or "Orange" or "Pomme":
                Poids.GetComponent<Rigidbody2D>().mass = 5;

                Poids.GetComponent<SpriteRenderer>().sprite = Context.itemSprite;
                Poids.GetComponent<BoxCollider2D>().size = new Vector2(4.78f, 6f);
                Poids.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                _poidAccumuler += 1;
                _stockIteam.Add(Context);
                break;
            case "GrosPot" or "PetitPot" or "vase":
                Poids.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
                Poids.GetComponent<Rigidbody2D>().mass = 10;
                _poidAccumuler += 1;
                _stockIteam.Add(Context);
                break;
            case "couronne" or "collier":
                Poids.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                Poids.GetComponent<Rigidbody2D>().mass = 45;
                _poidAccumuler += 1;
                _stockIteam.Add(Context);
                break;
            case "Améthyste":
                Poids.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
                Poids.GetComponent<Rigidbody2D>().mass = 90;
                _poidAccumuler += 1;
                _stockIteam.Add(Context);
                break;
            case "flute":
                Poids.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                Poids.GetComponent<Rigidbody2D>().mass = 100;
                _poidAccumuler += 1;
                _stockIteam.Add(Context);
                break;
        }


    }
    

}
