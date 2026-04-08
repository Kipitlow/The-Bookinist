using System;
using System.Collections;
using System.Collections.Generic; // Permet de faire un tableau
using TMPro;
using UnityEngine;

[Serializable]
public class List_Element_Tach
{
    [SerializeField] public string Nom_Mission;
    [SerializeField] public string Tache;
    [SerializeField] public bool TacheTerminer;
}


[Serializable]
public class UI_CacheLayeur
{
    [Header("Affiche UI & Objet dans un layeur")]
    public int Layeur_Affiche_Mission;
    public List<GameObject> CanvaUI;
    
}

public class SC_Tache : MonoBehaviour
{
    #region Variable
    [Header("Variable Utiliser pour le Chronometre")]
    [SerializeField] public TextMeshProUGUI Text_Chronom;
    private bool LanceCouroutine;
    //[SerializeField] public TextMeshProUGUI Text_Objectif; //////Objectif
    public int totalSeconds;

    [Header("Autre")]
    public Camera CM_Player;
    [SerializeField]public CameraMovement CM;
    public List<UI_CacheLayeur> UI_cacheLayeur = new List<UI_CacheLayeur>();
    private int Layeur_Actuelle_Du_Joueur;

    [Header("UI_Enigme_01")]
    public GameObject Balance;
    public GameObject CanvaMarchant;

    [Header("Prefable")]
    [SerializeField] public GameObject PrefableTache;
    [SerializeField] public GameObject Prefable_Canva_GameOver;
    [SerializeField] public Transform Target_Parent_Prefable;
    public List<GameObject> List_Temporair_Tache = new List<GameObject>(); //Permet de stocker les Prefable_Tache.

    [Header("Mission")]
    public List<List_Element_Tach> Liste_Mission = new List<List_Element_Tach>(); //Permet de stocker les Prefable_Tache.

    #endregion

    #region Unity Methods
    void Start()
    {
        if (CM_Player == null) CM_Player = GameObject.Find("CameraManager").GetComponent<Camera>();
        StartCoroutine("Chronometre"); //Permet de lancer la coroutine;
        Change_Tach_List();
    }
    void Update()
    {
        {
            if (CM_Player != null && Layeur_Actuelle_Du_Joueur != (int)Mathf.Round(CM_Player.transform.position.z) + 1 && Layeur_Actuelle_Du_Joueur != CM.currentIndexByLayer)  //Ce code consiste a v�rifier le layeur du joueur en fonction de sa position axe z et enfin de le terminer quand un changement est fait.     //&& Text_Objectif != null
            {
                Layeur_Actuelle_Du_Joueur = CM.currentIndexByLayer;
                // Cette option consiste a cacher tous les objets qui sont assigner a un layeur, on fonction du layeur du joueur cache le rester des objets.
                for (int i = 0; i < UI_cacheLayeur.Count; i++)
                {
                    if (i == Layeur_Actuelle_Du_Joueur)
                    {
                        foreach (GameObject CacheObjet in UI_cacheLayeur[i].CanvaUI)
                        {
                            if (CacheObjet != null) CacheObjet.SetActive(true);
                        }
                    }
                    else
                    {
                        foreach (GameObject CacheObjet in UI_cacheLayeur[i].CanvaUI)
                        {
                            if (CacheObjet != null) CacheObjet.SetActive(false);
                        }
                    }
                }
           }
        }
    }
    #endregion
        #region Methods

    public void Change_Tach_List()//CodePermettant de actualiser les objectif du joueur
    {
        //On fonction quand terminer la tache précédent on veut switch de tache.
        if (PrefableTache != null && Target_Parent_Prefable != null)
        {
            {
                // Code qui permet de supprimer tout préfable tache dans le code 
                foreach (GameObject obj in List_Temporair_Tache)
                {
                    if (obj != null) Destroy(obj);
                }
                List_Temporair_Tache.Clear();
                //Code permet d'afficher tous les mission terminer et une seul mission non terminer
                for (int i = 0; i < Liste_Mission.Count; i++)
                {
                    Spawn_Prefable_Tache(i);
                    if (Liste_Mission[i].TacheTerminer == false)
                    {
                        return; // la boucle ce terminer quand une tache n'est pas terminer
                    }
                }
            } //<- Actualiser les mission
            
        }
    }
    private void Spawn_Prefable_Tache(int i)               
    {
        GameObject New_Object = Instantiate(PrefableTache, Target_Parent_Prefable);
        Vector3 Pos = New_Object.transform.position;
        Pos.y = Pos.y - 100 * i;
        New_Object.transform.position = Pos;

        SC_Prefable_Tache Prefable_Script_Tache = New_Object.GetComponentInChildren<SC_Prefable_Tache>();
        //Dans ce code, on vêrifier si la tache en elle même est completer, si oui on change de couleur on rouge puis on le barre
        if (Prefable_Script_Tache != null)
        {
            if (Liste_Mission[i].TacheTerminer)
            {
                Prefable_Script_Tache.Text_Objectif.color = Color.red;
                Prefable_Script_Tache.Text_Objectif.text = $"<s>{Liste_Mission[i].Tache}</s>";
            }
            else if (!Liste_Mission[i].TacheTerminer)
            {
                Prefable_Script_Tache.Text_Objectif.color = Color.black;
                Prefable_Script_Tache.Text_Objectif.text = Liste_Mission[i].Tache;
                /*if (Liste_Mission[i].nbrTask + 1 < Liste_Mission[i].nbrTaskMax)
                {
                    Prefable_Script_Tache.Text_Objectif.text = Liste_Mission[i].Tache + $"({}/{})";
                }*/
            }
        }

        List_Temporair_Tache.Add(New_Object);
    }

    public void Terminer_Tache(string nom_mission)
    {
        for (int i = 0; i < Liste_Mission.Count; i++)
        {
            if (Liste_Mission[i].TacheTerminer == false)
            {
                if(Liste_Mission[i].Nom_Mission == nom_mission)
                {
                    Liste_Mission[i].TacheTerminer = true;
                    /*if(Liste_Mission[i].nbrTask+1 >= Liste_Mission[i].nbrTaskMax)
                    {
                        Liste_Mission[i].nbrTask = Liste_Mission[i].nbrTaskMax;
                        Liste_Mission[i].TacheTerminer = true;
                    }
                    else
                    {
                        Liste_Mission[i].nbrTask += 1;
                        Liste_Mission[i].TacheTerminer = false;
                    }*/
                    Change_Tach_List();
                }
                else
                {
                    Debug.LogWarning($"la mission {Liste_Mission[i].Nom_Mission} estr terminer");
                }
            }
        }
    }

    public void UpdateTache()
    {

    }


    private void Spawn_Canva_GameOver()
    {
        SC_UI_GameOver PUI = Prefable_Canva_GameOver.GetComponent<SC_UI_GameOver>();
        Transform GO_Canva = GameObject.Find("Canvas").transform;
        if (PUI != null && GO_Canva != null)
        {
            GameObject RR = Instantiate(Prefable_Canva_GameOver, transform.position, transform.rotation);
            RR.transform.SetParent(GO_Canva, false); // Permet d'annuler

            RR.transform.localScale = new Vector2(0.5f, 0.5f);

            RectTransform rt = RR.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(transform.position.x / 2, transform.position .y/ 2);
        }
    }

    public void Affiche_Marchant(GameObject Self)
    {
        if (Self != null) Self.SetActive(false);

        if (Balance != null)
        {
            Balance.SetActive(!Balance.activeSelf);
            Debug.Log($"Trouver Balance: {Balance.activeSelf} "); 
        }
        else 
        { 
            Debug.LogWarning("Erreur du system Balance =null"); 
        }
            

        if (CanvaMarchant != null)
        {
            CanvaMarchant.SetActive(!CanvaMarchant.activeSelf);
            Debug.Log($"Trouver Balance: {CanvaMarchant.activeSelf} ");
        }
        else 
        {
            Debug.LogWarning("Erreur du system CanvaMarchant =null");
        }
    }
    // Permet de faire un chronomêttre
    IEnumerator Chronometre()
    {
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
            totalSeconds = 0;
            Text_Chronom.text = $"{totalSeconds / 60}:{totalSeconds % 60}";
            LanceCouroutine = false;
            Spawn_Canva_GameOver();
            StopCoroutine("Chronometre");
        }
    } 
    //Cette fonction ci-dessous est utiliser dans un event bouton, et permet de mêttre pause le chrono !!! Attention elle ne stop pas le déroulement dans la scéne
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
    #endregion
}
