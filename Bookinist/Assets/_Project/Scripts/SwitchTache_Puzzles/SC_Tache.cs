using System;
using System.Collections;
using System.Collections.Generic; // Permet de faire un tableau
using System.Runtime.CompilerServices; // Permet d'utiliser une Coroutine
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class List_Element_Tach
{
    [SerializeField] public string Nom_Mission;
    [SerializeField] public string Tache;
    [SerializeField] public int prix;
    [SerializeField] public bool TacheTerminer;
}


[Serializable]
public class Tache_Layourt
{
    [Header("Inscription Tache")]
    public int Layeur_Affiche_Mission;
    public List<List_Element_Tach> list_Element_Taches;
    
}

public class SC_Tache : MonoBehaviour
{
    [Header("Variable Utiliser pour le Chronom�tre")]
    [SerializeField] public TextMeshProUGUI Text_Chronom;
    //[SerializeField] public TextMeshProUGUI Text_Objectif; //////Objectif
    public int totalSeconds;

    [Header("Prefable")]
    [SerializeField] public GameObject PrefableTache;
    [SerializeField] public Transform Target_Parent_Prefable;
    public List<GameObject> List_Temporair_Tache = new List<GameObject>();


    [Header("Autre")]
    public Camera CM_Player;
    [SerializeField]public CameraMovement CM;
    public Tache_Layourt[] Tache_Dans_Ce_Layeur;
    private int WaitCondition;


    void Start()
    {
        if (CM_Player == null) CM_Player = GameObject.Find("CameraManager").GetComponent<Camera>();
        StartCoroutine("Chronometre"); //Permet de lancer la coroutine;
        UpdateQuestText();
    }

    void Update()
    {
        if (CM_Player != null && WaitCondition != (int)Mathf.Round(CM_Player.transform.position.z) + 1 && WaitCondition!= CM.currentIndexSnapPoint)  //Ce code consiste a v�rifier le layeur du joueur en fonction de sa position axe z et enfin de le terminer quand un changement est fait.     //&& Text_Objectif != null
        {
            
            foreach (Tache_Layourt EE in Tache_Dans_Ce_Layeur)     //Nous utilison le curentIndex""" pour savoir sur quels layeur se trouver le joueur, si celle si posséder une tache, la mission change
            {
                if(Tache_Dans_Ce_Layeur[CM.currentIndexSnapPoint] != null && EE.Layeur_Affiche_Mission == CM.currentIndexSnapPoint)
                {
                    WaitCondition = CM.currentIndexSnapPoint;
                    Change_Tach_List();
                }
            }
        }
    }

    void Change_Tach_List()//CodePermettant de actualiser les objectif
    {
        //Supprimer tous les taches present dans la list.
        foreach (GameObject obj in List_Temporair_Tache)
        {
            if (obj != null) Destroy(obj);
        }
        List_Temporair_Tache.Clear(); //On n'oublie pas de clear si on veux liberais de la place
        
        //En vêrifier tout les tache dans le layeur
        if (PrefableTache != null && Target_Parent_Prefable != null)
        {
            int Repeat = 0;
            foreach (List_Element_Tach iii in Tache_Dans_Ce_Layeur[WaitCondition].list_Element_Taches)
            {
                if(Tache_Dans_Ce_Layeur[WaitCondition].list_Element_Taches != null)
                {
                    GameObject AA = Instantiate(PrefableTache, Target_Parent_Prefable);
                    Vector3 Pos = AA.transform.position;
                    Pos.y = Pos.y - 100 * Repeat;
                    AA.transform.position = Pos;

                    SC_Prefable_Tache TT = AA.GetComponentInChildren<SC_Prefable_Tache>();
                    if(TT != null)
                    {

                        if (iii.TacheTerminer)
                        {
                            TT.Text_Objectif.color = Color.red;
                            TT.Text_Objectif.text = $"<s>{iii.Tache}</s>";
                            TT.Text_Récompence.text = $"Récompence: {iii.prix}$";
                        }
                        else if (!iii.TacheTerminer)
                        {
                            TT.Text_Objectif.color = Color.black;
                            TT.Text_Objectif.text = iii.Tache;
                            TT.Text_Récompence.text = $"Récompence: {iii.prix}$";
                        }
                    }

                    List_Temporair_Tache.Add(AA);
                    Repeat += 1;
                }
            }
        }
    }
    IEnumerator Chronometre()
    {
        float Position_x;
        if (totalSeconds - 1 >= 1)
        {
            totalSeconds -= 1;
            Text_Chronom.text = $"{totalSeconds / 60}:{totalSeconds % 60}";

            yield return new WaitForSeconds(1);
            StartCoroutine("Chronometre");
        }
        else
        {
            StopCoroutine("Chronometre");
        }
    }

    private void UpdateQuestText()               
    {
        /*Text_Objectif.text = Tache_Dans_Ce_Layeur[0].GetObjectif();        < ---------------------------------------------*/
    }
}
