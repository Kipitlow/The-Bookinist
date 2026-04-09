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
    #endregion


    void Start()
    {
        _targetOne=true;    
    }

    private void Update()
    {
        if (balance == null) return;
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
    /*public void Cache_Balance(GameObject Self)
    {
        switch (Self.name)
        {
            case "B_balance":
                CacheCetteObject(Self);
                SpawnerPoidsBalance(0);
                break;
            case "B_Reset":
                SpawnerPoidsBalance(0);
                foreach (GameObject aa in Button_Hidden)
                {
                    if (aa.name == "B_balance") aa.SetActive(true);
                    else aa.SetActive(false);
                }
                break;
            case "B_Object_1":
                CacheCetteObject(Self);
                SpawnerPoidsBalance(1);
                break;
            case "B_Object_2":
                CacheCetteObject(Self);
                Invoke("change_UI", 2);
                SpawnerPoidsBalance(2);      // <--- c'est ici que l'égnime prend fin    
                break;
            case "B_Object_3":
                CacheCetteObject(Self);
                SpawnerPoidsBalance(3);
                break;
            case "B_Object_4":
                CacheCetteObject(Self);
                SpawnerPoidsBalance(4);
                break;
            case "B_Object_5":
                CacheCetteObject(Self);
                SpawnerPoidsBalance(5);
                break;
        }
    }*/
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


        Transform EE = GameObject.Find("Target_spawn_Poids_R").transform;
        Poids = Instantiate(Prefable_Poids, EE);

        if (Poids.GetComponent<Rigidbody>() != null) { Debug.LogError("Pourquoi ce foutus de ce RigBody"); }

        EE = GameObject.Find("Target_spawn_Poids_L").transform;
        if (_targetOne==true)
        {
            _targetOne=false;   
            Poids2 = Instantiate(Prefable_Poids, EE);
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
    

    //Fonction "CacheCetteObject" consister a montrer tout le contenue de Button_Hidden tout on cachant l'un des boutton
    /*private void CacheCetteObject(GameObject ee) 
    {
        foreach(GameObject aa in Button_Hidden)
        {
            if(aa.name !="B_balance") aa.SetActive(true);
        }
        ee.SetActive(false);
    }*/

    // Fonction "change_UI" permet d'intervertire entre le canva est celui du marchant.
    /*public void change_UI()
    {
        //Ce code: consister a mieux controller qu'elle GameObject actif et d'autre non
        switch (Icone_Apparaition)
        {
            case true: // on veut voir l'icone "Marchant" pas le reste
                Icone_Marchant.SetActive(true);
                E_Marchant.SetActive(false);
                object_Balance.SetActive(false);
                Icone_Apparaition = false;
                break;
            case false: // on veut voir la balance est l'UI, mais cache l'icone "Marchant
                Icone_Marchant.SetActive(false);
                E_Marchant.SetActive(true);
                object_Balance.SetActive(true);
                Icone_Apparaition = true;

                //Cette condition on veut faire disparait le bouton, et non le destroy;
                if (b_spawn_Marchant != null)
                {
                    b_spawn_Marchant.onClick.RemoveListener(change_UI);
                    b_spawn_Marchant.gameObject.SetActive(false);
                }
                break;
        }

    }*/

    /*public void Remove_Listener_Function(UnityEngine.Events.UnityAction call) // Ou c'est appeller ceci?
    {
        if (b_spawn_Marchant != null) 
        {
            b_spawn_Marchant.onClick.RemoveListener(call);

        } 
    }*/



    //Cette Fonction "tache_terminer" permet de valider la mission a condition que le nom donner a la fonction corresponde au nom de la tache. //Pour tout explication claire, consulter Vatea
    /*public void tache_terminer()
    {
        //vêrifier un tableau puis une liste, oui j'ai pas fait plus simple, Normalement vous devez donner un nom a tout mission, pour indentifier quels mission surligner
        if (Script_Tache != null)
        {
            foreach (Tache_Layourt TL in Script_Tache.Tache_Dans_Ce_Layeur)
            {
                foreach (List_Element_Tach LET in TL.list_Element_Taches)
                {
                    if (Name_Tach_Self == LET.Nom_Mission)
                    {
                        Debug.Log("Mission Egnime 1 Terminer");
                        LET.TacheTerminer = true;
                        Script_Tache.Change_Tach_List();
                    }
                }
            }
        }
    }*/
}
