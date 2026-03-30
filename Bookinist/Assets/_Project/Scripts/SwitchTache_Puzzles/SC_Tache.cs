using System;
using System.Collections;
using System.Collections.Generic; // Permet de faire un tableau
using System.Runtime.CompilerServices; // Permet d'utiliser une Coroutine
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    public List<GameObject> CanvaUI;
    
}

public class SC_Tache : MonoBehaviour
{
    [Header("Variable Utiliser pour le Chronometre")]
    [SerializeField] public TextMeshProUGUI Text_Chronom;
    private bool LanceCouroutine;
    //[SerializeField] public TextMeshProUGUI Text_Objectif; //////Objectif
    public int totalSeconds;

    [Header("Prefable")]
    [SerializeField] public GameObject PrefableTache;
    [SerializeField] public Transform Target_Parent_Prefable;
    public List<GameObject> List_Temporair_Tache = new List<GameObject>();


    [Header("Autre")]
    public Camera CM_Player;
    [SerializeField]public CameraMovement CM;
    public List<Tache_Layourt> Tache_Dans_Ce_Layeur = new List<Tache_Layourt>();
    //public Tache_Layourt[] Tache_Dans_Ce_Layeur;
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

            WaitCondition = CM.currentIndexSnapPoint;
            
            {
                foreach (GameObject obj in List_Temporair_Tache)
                {
                    if (obj != null) Destroy(obj);
                }
                List_Temporair_Tache.Clear();
            } // !!! A Pas Supprimer: Code qui permet de supprimer tout préfable tache dans le code 


            foreach (Tache_Layourt Layeur in Tache_Dans_Ce_Layeur)     //Nous utilison le curentIndex""" pour savoir sur quels layeur se trouver le joueur, si celle si posséder une tache, la mission change
            {
                if (Layeur !=null && Layeur.Layeur_Affiche_Mission == CM.currentIndexSnapPoint)
                {                    
                    Change_Tach_List();
                }
            }
        }
    }

    public void Change_Tach_List()//CodePermettant de actualiser les objectif du joueur
    {        
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
                    //Dans ce code, on vêrifier si la tache en elle même est completer, si oui on change de couleur on rouge puis on le barre
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
            for (int i = 0; i<Tache_Dans_Ce_Layeur.Count; i++)
            {
                if(i == WaitCondition)
                {
                    foreach (GameObject CacheObjet in Tache_Dans_Ce_Layeur[i].CanvaUI)
                    {
                        if(CacheObjet!=null) CacheObjet.SetActive(true);
                    }
                }
                else
                {
                    foreach (GameObject CacheObjet in Tache_Dans_Ce_Layeur[i].CanvaUI)
                    {
                        if (CacheObjet != null) CacheObjet.SetActive(false);
                    }
                }
            }// Cette option consiste a cacher tous les objets qui sont assigner a un layeur, on fonction du layeur du joueur cache le rester des objets.
        }


        Debug.Log("Switch Canva");
    }
    IEnumerator Chronometre()
    {
        float Position_x;
        if (totalSeconds - 1 >= 1)
        {
            totalSeconds -= 1;
            Text_Chronom.text = $"{totalSeconds / 60}:{totalSeconds % 60}";

            yield return new WaitForSeconds(1);
            LanceCouroutine = true;
            StartCoroutine("Chronometre");
        }
        else
        {
            LanceCouroutine = false;
            StopCoroutine("Chronometre");
        }
    } // Permet de faire un chronomêttre

    private void UpdateQuestText()               
    {
        //Text_Objectif.text = Tache_Dans_Ce_Layeur[0].GetObjectif();        <--- Je n'est pas utiliser, demander de le modifier ou supprimer si elle n'est point utiliser.
    }

    public void spawn_UI_Marchant(GameObject Prefable_UI)
    {
        SC_Marchant PUI = Prefable_UI.GetComponent<SC_Marchant>();
        Transform GO_Canva = GameObject.Find("Canvas").transform;
        if (PUI!=null && GO_Canva != null)
        {
            GameObject RR = Instantiate(Prefable_UI, GO_Canva);
            RR.transform.SetParent(GO_Canva, false); // Permet d'annuller

            RectTransform rt = RR.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, 0);

            SC_Marchant SCM = RR.GetComponentInChildren<SC_Marchant>();
            //SCM.change_UI();
        }
    }   /// <------ A plus utiliser
    public void Affiche_Marchant()
    {
        GameObject Balance = GameObject.Find("Balance2D");
        if (Balance != null) Balance.SetActive(!Balance.activeSelf);
        GameObject CanvaMarchant = GameObject.Find("E_Marchant");
        if (CanvaMarchant != null) CanvaMarchant.SetActive(!CanvaMarchant.activeSelf);
    }

    public void SetChronom(bool Continue)
    {
        if(Continue)
        {
            if (!LanceCouroutine) 
            { 
                StartCoroutine("Chronometre");
                LanceCouroutine = true;
            }
        }
        else if (!Continue)
        {
            if (LanceCouroutine)
            {
                StopCoroutine("Chronometre");
                LanceCouroutine = false;
            }
        }
    }
}
