using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static UnityEditor.ShaderData;

public class Dialogue_Marchant
{
    public string T_Dialogue;

}
public class SC_Marchant : MonoBehaviour
{
    [Header("Gestion UI")]
    //[SerializeField] public GameObject UI_Gameplay;
    public GameObject Prefable_Poids;
    private SC_Tache Script_Tache;
    private GameObject E_Marchant;
    private GameObject Balance;
    private GameObject Icone_Marchant;
    private Button B_Marchant;

    [Header("Autre")]
    [SerializeField] public GameObject[] Button_Hidden;
    private GameObject Poids;
    private GameObject Poids2;

    [Header("RenameTache")]
    private string Name_Tach_Self;


    void Start()
    {
        Icone_Marchant = GameObject.Find("Icone_PNJ_Marchant");
        E_Marchant = GameObject.Find("E_Marchant");
        Balance = GameObject.Find("Balance2D");
            
        Script_Tache = GameObject.Find("Canvas").GetComponent< SC_Tache>();
        B_Marchant = GameObject.Find("B_Spawn_Marchant").GetComponent<Button>();

        change_UI();

        foreach (GameObject aa in Button_Hidden)
        {
            if(aa.name== "B_balance")
            {
                aa.SetActive(true);
            }
            else
            {
                aa.SetActive(false);
            }
        }
        Name_Tach_Self = "Egnigme_01";
    }


    /*private void Add_Mission()
    {
        if (Script_Tache != null)
        {
            //Construit Une tache Avec les Elements
            List_Element_Tach Richart = new List_Element_Tach();
            Name_Tach_Self = "Egnigme_01";
            Richart.Nom_Mission = Name_Tach_Self;
            Richart.Tache = "Trouve le Marchant, et ";
            Richart.prix = 50;
            Richart.TacheTerminer = false;

            int LayeurNbr=0;// !Attention veuiller définir dans quels layeurs il apparait. Veiller que 2 mission ne s'affiche par dans le même layeur se deviendrer soulant.

            if (Script_Tache.Tache_Dans_Ce_Layeur.Count <= LayeurNbr)
            {
                Debug.Log("A1");
                Script_Tache.Tache_Dans_Ce_Layeur[LayeurNbr].list_Element_Taches.Add(Richart);
            }
            else
            {
                Debug.Log("A2");
                Tache_Layourt TL = new Tache_Layourt();
                TL.Layeur_Affiche_Mission = LayeurNbr;
                TL.list_Element_Taches.Add(Richart);

                Script_Tache.Tache_Dans_Ce_Layeur.Add(TL);
            }
        }
        else { Debug.LogWarning("Erreur Script_Tache = null"); }
    }*/


    public void Cache_Balance(GameObject Self)
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
                tache_terminer();
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
    }
    public void SpawnerPoidsBalance(int Mass)
    {
        if (Poids != null)
        {
            Destroy(Poids);
            Poids=null;
        }
        if (Poids2 != null)
        {
            Destroy(Poids2);
            Poids2 = null;
        }


        Transform EE = GameObject.Find("Target_spawn_Poids_R").transform;
        Poids = Instantiate(Prefable_Poids, EE);

        if (Poids.GetComponent<Rigidbody>() != null) { Debug.LogError("Pourquoi ce foutus de ce RigBody"); }

        EE = GameObject.Find("Target_spawn_Poids_L").transform;
        Poids2 = Instantiate(Prefable_Poids, EE);
        Poids2.GetComponent<Rigidbody2D>().mass = 100;
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
    private void CacheCetteObject(GameObject ee) 
    {
        foreach(GameObject aa in Button_Hidden)
        {
            if(aa.name !="B_balance") aa.SetActive(true);
        }
        ee.SetActive(false);
    }

        // Fonction "change_UI" permet d'intervertire entre le canva est celui du marchant.
    public void change_UI()
    {
        


        if (B_Marchant != null && E_Marchant != null && E_Marchant.activeSelf && Balance != null && E_Marchant != null && Icone_Marchant!=null)
        {
            B_Marchant.onClick.RemoveListener(change_UI);
            B_Marchant.gameObject.SetActive(false);

            E_Marchant.SetActive(!E_Marchant.activeSelf);
            Balance.SetActive(!E_Marchant.activeSelf);
            //Icone_Marchant.SetActive(!E_Marchant.activeSelf);
        }
            
    }

    public void Remove_Listener_Function(UnityEngine.Events.UnityAction call)
    {
        if (B_Marchant != null) 
        {
            B_Marchant.onClick.RemoveListener(call);

        } 
    }


    //Cette Fonction "tache_terminer" permet de valider la mission a condition que le nom donner a la fonction corresponde au nom de la tache.
    //Pour tout explication claire, consulter Vatea
    public void tache_terminer()
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
                        //ICI que la mission ce terminer, est donc de récompencer c'est joueur.
                    }
                }
            }
        }
    }
}
