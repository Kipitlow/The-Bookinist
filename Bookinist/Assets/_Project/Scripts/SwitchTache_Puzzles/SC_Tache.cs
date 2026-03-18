using System;
using System.Collections;
using System.Runtime.CompilerServices; // Permet d'utiliser une Coroutine
using TMPro;
using UnityEngine;
using System.Collections.Generic; // Permet de faire un tableau


[Serializable]
public class Tache_Layourt
{
    [Header("Inscription Tache")]
    [SerializeField] public string[] Objectif;
}

public class SC_Tache : MonoBehaviour
{
    [Header("Variable Utiliser pour le Chronomêtre")]
    [SerializeField] public TextMeshProUGUI Text_Chronom;
    [SerializeField] public TextMeshProUGUI Text_Objectif; //////Objectif
    public int totalSeconds;

    [Header("Prefable")]
    [SerializeField] public GameObject PrefableTache;
    [SerializeField] public Transform Target_Parent_Prefable;
    private List<GameObject> List_Temporair_Tache = new List<GameObject>();
    //private GameObject[] List_Temporair_Tache;

    [Header("Autre")]
    public Camera CM_Player;
    public Tache_Layourt[] Tache_Dans_Ce_Layeur;
    private int WaitCondition;


    void Start()
    {
        if(CM_Player==null)CM_Player = GameObject.Find("CameraManager").GetComponent<Camera>();
        StartCoroutine("Chronometre"); //Permet de lancer la coroutine;
        
    }
    void Update()
    {

        if (CM_Player != null && WaitCondition != (int)Mathf.Round(CM_Player.transform.position.z) + 1)  //Ce code consiste a vêrifier le layeur du joueur en fonction de sa position axe z et enfin de le terminer quand un changement est fait.     //&& Text_Objectif != null
        {//Text_Objectif.text = $"Layeur: {Mathf.Round(CM_Player.transform.position.z / 20)}";

            WaitCondition = (int)Mathf.Round(CM_Player.transform.position.z / 20) + 1;
            Change_Tach_List();
        }

    }

    void Change_Tach_List()
    {
        //CodePermettant De supprimer tous les taches present dans la list.
        foreach (GameObject obj in List_Temporair_Tache)
        {
            if(obj!=null)Destroy(obj);
        }
        List_Temporair_Tache.Clear(); //On n'oublie pas de clear si on veux liberais de la place

        
        if (PrefableTache != null && Target_Parent_Prefable != null)
        {
            int Repeat = 0;
            foreach (string iii in Tache_Dans_Ce_Layeur[WaitCondition].Objectif)
            {
                
                GameObject AA = Instantiate(PrefableTache, Target_Parent_Prefable);
                Vector3 Pos = AA.transform.position;
                Pos.y = Pos.y - 100* Repeat;
                AA.transform.position = Pos;
                
                
                TextMeshProUGUI TT = AA.GetComponentInChildren<TextMeshProUGUI>();
                TT.text = iii;
                List_Temporair_Tache.Add(AA);
                Repeat += 1;
            }
        }
    }
    IEnumerator Chronometre()
    {
        float Position_x;
        if(totalSeconds-1 >=1)
        {
            totalSeconds-=1;
            Text_Chronom.text = $"Time: {totalSeconds / 60}:{totalSeconds % 60}s";
            
            yield return new WaitForSeconds(1);
            StartCoroutine("Chronometre");
        }
        else
        {
            StopCoroutine("Chronometre");
        }
    }
}
