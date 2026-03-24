using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Dialogue_Marchant
{
    public string T_Dialogue;

}
[Serializable]
public class GestionUI
{
    
}
public class SC_Marchant : MonoBehaviour
{
    [Header("Gestion UI")]
    //[SerializeField] public GameObject UI_Gameplay;
    private SC_Tache Script_Tache;
    private GameObject E_Marchant;
    private Button B_Marchant;

    [Header("Autre")]
    [SerializeField] public GameObject[] Button_Hidden;
    private Animator an;


    void Start()
    {
        an = GetComponentInChildren<Animator>();

        E_Marchant = GameObject.Find("E_Marchant"); if (E_Marchant == null) print("<E_Marchant> = null");
            
        Script_Tache = GameObject.Find("Canvas").GetComponent< SC_Tache>();
        if(Script_Tache == null) { print("Erreur Script non trouver"); }

        B_Marchant = GameObject.Find("B_Spawn_Marchant").GetComponent<Button>();
        if(B_Marchant==null) { print("Erreur Button Spawn disappeared"); }

        foreach (GameObject aa in Button_Hidden)
            {
                if (aa.name == "B_balance") aa.SetActive(true);
                else aa.SetActive(false);
            }


    }


    public void Cache_Balance(GameObject Self)
    {
        switch (Self.name)
        {
            case "B_balance":
                CacheCetteObject(Self);
                break;
            case "B_Reset":
                foreach (GameObject aa in Button_Hidden)
                {
                    if (aa.name == "B_balance") aa.SetActive(true);
                    else aa.SetActive(false);
                }
                break;
            case "B_Object_1":
                CacheCetteObject(Self);
                change_An_Balance(-1);
                break;
            case "B_Object_2":
                CacheCetteObject(Self);
                change_An_Balance(0);
                tache_terminer("Marchant");
                Invoke("change_UI", 2);       // <--- c'est ici que l'égnime prend fin    
                break;
            case "B_Object_3":
                CacheCetteObject(Self);
                change_An_Balance(1);
                break;
            case "B_Object_4":
                CacheCetteObject(Self);
                change_An_Balance(2);
                break;
            case "B_Object_5":
                CacheCetteObject(Self);
                change_An_Balance(0);
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


    //Fonction "change_An_Balance" Permet de changer l'animation de la balance
    private void change_An_Balance(int rr)
    {
        an.SetInteger("Enumeration", rr);
    }



    // Fonction "change_UI" permet d'intervertire entre le canva est celui du marchant.
    public void change_UI()
    {
        if (E_Marchant != null)  E_Marchant.SetActive(!E_Marchant.activeSelf);
        if (B_Marchant != null && E_Marchant.activeSelf == true) 
        {
            print("Yep");
            B_Marchant.onClick.RemoveListener(change_UI);
            B_Marchant.gameObject.SetActive(false);
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
    public void tache_terminer(string Name_Tache)
    {
        //vêrifier un tableau puis une liste, oui j'ai pas fait plus simple, Normalement vous devez donner un nom a tout mission, pour indentifier quels mission surligner
        if (Script_Tache != null)
        {
            foreach (Tache_Layourt TL in Script_Tache.Tache_Dans_Ce_Layeur)
            {
                foreach (List_Element_Tach LET in TL.list_Element_Taches)
                {
                    if (Name_Tache == LET.Nom_Mission)
                    {
                        LET.TacheTerminer = true;
                        //ICI que la mission ce terminer, est donc de récompencer c'est joueur.
                    }
                }
            }
        }
    }
}
