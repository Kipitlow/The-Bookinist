using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ScriptBalance : MonoBehaviour
{
    #region Variable
    [Header("Gestion des sprite de la Balance")]
    //[SerializeField] public GameObject UI_Gameplay;
    public GameObject balanceSprite;
    public GameObject balance;
    public GameObject plateau1;
    public GameObject plateau2;

    [Header("Prefable GameObject")]
    //[SerializeField] public GameObject UI_Gameplay;
    public GameObject prefable_Poids;
    public GameObject prefableLyre;
    public Item lyreSTP;

    [Header("Interne variable")]
    private List<GameObject> _listPoids = new();
    private List<Item> _stockIteam = new();
    /*
    private GameObject _poids;
    private GameObject _poids2;
    */
    private bool _targetOne; //permet de d'instantier le deuxiéme poids "c'est la lyre"
    private int _poidAccumuller=0;
    private int _poidAutoriser=0;
    private GameObject _2emePoids;

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
        Vector3 _v3 = new Vector3(transform.position.x, transform.position.y + 2.6f, transform.position.z);
        GameObject ee = Instantiate(prefableLyre, transform);
        ee.transform.position = _v3;

        for (int i = 0; i < _listPoids.Count; i++)
        {
            if (_listPoids[i] != null)
            {
                Destroy(_listPoids[i]);
                _listPoids[i] = null;
            }
        }
    }
    /*public void SpawnerPoidsBalance(int Mass)
    {


        Transform _ballMass = GameObject.Find("Target_spawn_Poids_R").transform;
        _poids = Instantiate(prefable_Poids, _ballMass);

        if (_poids.GetComponent<Rigidbody>() != null) { Debug.LogError("Pourquoi ce foutus de ce RigBody"); }

        _ballMass = GameObject.Find("Target_spawn_Poids_L").transform;
        if (_targetOne==true)
        {
            _targetOne=false;   
            _poids2 = Instantiate(prefable_Poids, _ballMass);
            _poids2.GetComponent<Rigidbody2D>().mass = 100;
        }
        
        
        switch (Mass)
        {
            case 0:
                if (_poids != null)
                {
                    Destroy(_poids);
                    _poids = null;
                }
                break;
            case 100:
                _poids.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                _poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
            case 2:
                _poids.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
                _poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
            case 3:
                _poids.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                _poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
            case 4:
                _poids.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
                _poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
            case 5:
                _poids.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                _poids.GetComponent<Rigidbody2D>().mass = Mass;
                break;
        }


    }*/
    
    public void SpawnerIteamBalance(Item Context )
    {
        Debug.Log("------------------Spawn Fonction");
        if (_poidAutoriser >= 3) {  return; }
        Transform _ballMass = GameObject.Find("Target_spawn_Poids_R").transform;
        GameObject _1erPoids = Instantiate(prefable_Poids, _ballMass);
        _listPoids.Add(_1erPoids);
        if (_1erPoids.GetComponent<Rigidbody>() != null) { Debug.LogError("Pourquoi ce foutus de ce RigBody"); }

        _ballMass = GameObject.Find("Target_spawn_Poids_L").transform;
        if (_targetOne==true)
        {
            _targetOne=false;
            _2emePoids = Instantiate(prefable_Poids, _ballMass);
            
            _2emePoids.GetComponent<Rigidbody2D>().mass = 100;
            _2emePoids.GetComponent<SpriteRenderer>().sprite = lyreSTP.itemSprite;
            _2emePoids.GetComponent<BoxCollider2D>().size = new Vector2(4.78f, 6f);
            _2emePoids.transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
        }


        switch (Context.name)
        {
            case "":
                if (_1erPoids != null)
                {
                    Destroy(_1erPoids);
                    _poidAutoriser =0;
                    _1erPoids = null;
                }
                break;
            case "Citron" or "Figue" or "Orange" or "Pomme":
                _1erPoids.GetComponent<Rigidbody2D>().mass = 5;

                _1erPoids.GetComponent<SpriteRenderer>().sprite = Context.itemSprite;
                _1erPoids.GetComponent<BoxCollider2D>().size = new Vector2(4.78f, 6f);
                _1erPoids.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);

                _poidAccumuller += 5;
                _poidAutoriser += 1;
                _stockIteam.Add(Context);
                break;
            case "GrosPot" or "PetitPot" or "vase": //Pug SpriteRenderer Vase: n'apparait pas
                _1erPoids.GetComponent<Rigidbody2D>().mass = 10;

                _1erPoids.GetComponent<SpriteRenderer>().sprite = Context.itemSprite;
                _1erPoids.GetComponent<BoxCollider2D>().size = new Vector2(4f, 5f);
                _1erPoids.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                _poidAccumuller += 10;
                _poidAutoriser += 1;
                _stockIteam.Add(Context);
                break;
            case "couronne":
                _1erPoids.GetComponent<Rigidbody2D>().mass = 45;

                _1erPoids.GetComponent<SpriteRenderer>().sprite = Context.itemSprite;
                _1erPoids.GetComponent<BoxCollider2D>().size = new Vector2(19f, 10f);
                _1erPoids.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 1.27f);
                _1erPoids.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                _poidAccumuller += 45;
                _poidAutoriser += 1;
                _stockIteam.Add(Context);
                break;
            case "collier":
                _1erPoids.GetComponent<Rigidbody2D>().mass = 45;

                _1erPoids.GetComponent<SpriteRenderer>().sprite = Context.itemSprite;
                _1erPoids.GetComponent<BoxCollider2D>().size = new Vector2(4f, 3.64f);
                _1erPoids.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0.2f);
                _1erPoids.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                _poidAccumuller += 45;
                _poidAutoriser += 1;
                _stockIteam.Add(Context);
                break;
            case "Améthyste":
                _1erPoids.GetComponent<Rigidbody2D>().mass = 90;

                _1erPoids.GetComponent<SpriteRenderer>().sprite = Context.itemSprite;
                _1erPoids.GetComponent<BoxCollider2D>().size = new Vector2(22f, 15f);
                _1erPoids.GetComponent<BoxCollider2D>().offset = new Vector2(2f, 1.3f);
                _1erPoids.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);

                _poidAccumuller += 90;
                _poidAutoriser += 1;
                _stockIteam.Add(Context);
                break;
            case "flute":
                _1erPoids.GetComponent<Rigidbody2D>().mass = 100;

                _1erPoids.GetComponent<SpriteRenderer>().sprite = Context.itemSprite;
                _1erPoids.GetComponent<BoxCollider2D>().size = new Vector2(4.25f, 27.3f);
                _1erPoids.GetComponent<BoxCollider2D>().offset = new Vector2(.9f, 0f);
                _1erPoids.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                _poidAccumuller += 100;
                _poidAutoriser += 1;
                _stockIteam.Add(Context);
                break;
        }

        if (_poidAccumuller >= 100)
        {
            GameObject Chiant_Variable = GameObject.Find("B_Reset_Iteam_Balance");
            if (Chiant_Variable) Destroy(Chiant_Variable);
            Invoke("FinishTask", 1);
        }
    }
    
    public void resetBalance()
    {
        _targetOne = true;
        _poidAutoriser = 0;
        _poidAccumuller = 0;
        balance.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        StartCoroutine(_setItem());
    }
    IEnumerator _setItem()
    {
        InventoryController inventoryController = GameObject.Find("Canvas").GetComponentInChildren<InventoryController>() ;
        if(inventoryController!=null) 
        {
            for(int i=0; i<_stockIteam.Count; i++)
            {
                Debug.Log($"Place: {inventoryController.IsInventoryHasPlace()}");
                if (_listPoids[i] != null && _stockIteam[i] != null && inventoryController.IsInventoryHasPlace())//&& inventoryController._inventoryContents
                {
                    inventoryController.AddInventoryItem(_stockIteam[i]);
                    Destroy(_listPoids[i]);
                    _poidAutoriser -= 1;
                }
                Destroy(_2emePoids);
                //if (_stockIteam[i].itemName == "Lyre") Destroy(_stockIteam[i]);
                yield return new WaitForSeconds(0.1f);
            }

            /*while (_iteamRestant <= _listPoids.Count)
            {
                
                if (_listPoids[_iteamRestant] != null && _stockIteam[_iteamRestant] != null)//&& inventoryController._inventoryContents
                {
                    inventoryController.AddInventoryItem(_stockIteam[_iteamRestant]);
                    Destroy(_listPoids[_iteamRestant]);
                    *//*_stockIteam[_iteamRestant] = null;
                    _listPoids[_iteamRestant] = null;*//*
                    _poidAutoriser -= 1;
                }
                if (_stockIteam[_iteamRestant].itemName == "Lyre") Destroy(_stockIteam[_iteamRestant]); 
                _iteamRestant += 1;
                yield return new WaitForSeconds(0.1f);
            }*/
                //_stockIteam.Clear();
        }  
    }
}
