using System;
using System.Collections;
using System.Collections.Generic; // Permet de faire un tableau
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class List_Element_Tach
{
    public string nomMission;
    public string tache;
    public bool tacheTerminer;
}

public class SC_Tache : MonoBehaviour
{
    #region Variable
    [Header("Variable Utiliser pour le Chronometre")]
    public TextMeshProUGUI textChronom;
    private bool _lanceCouroutine;
    //[SerializeField] public TextMeshProUGUI Text_Objectif; //////Objectif
    public int totalSeconds;

    /*[Header("Autre")]
    public Camera CM_Player;
    [SerializeField]public CameraMovement CM;*/
    //private int Layeur_Actuelle_Du_Joueur;

    [Header("UI_Enigme_02")]
    public int nombreTacheValide=0;

    [Header("Prefable")]
    public GameObject prefableTache;
    public GameObject prefableCanvaGameOver;
    public Transform targetParentPrefable;
    public List<GameObject> listTemporairTache = new List<GameObject>(); //Permet de stocker les Prefable_Tache.

    [Header("Mission")]
    public List<List_Element_Tach> listeMission = new List<List_Element_Tach>(); //Permet de stocker les Prefable_Tache.

    #endregion

    #region Unity Methods
    void Start()
    {
        //if (CM_Player == null) CM_Player = GameObject.Find("CameraManager").GetComponent<Camera>();
        StartCoroutine("Chronometre"); //Permet de lancer la coroutine;
        ChangeTacheList();
    }
    #endregion

    #region Methods

    public void ChangeTacheList()//CodePermettant de actualiser les objectif du joueur
    {
        //On fonction quand terminer la tache précédent on veut switch de tache.
        if (prefableTache != null && targetParentPrefable != null)
        {
            {
                // Code qui permet de supprimer tout préfable tache dans le code 
                foreach (GameObject obj in listTemporairTache)
                {
                    if (obj != null) Destroy(obj);
                }
                listTemporairTache.Clear();
                //Code permet d'afficher tous les mission terminer et une seul mission non terminer
                for (int i = 0; i < listeMission.Count; i++)
                {
                    SpawnPrefableTache(i);
                    if (listeMission[i].tacheTerminer == false)
                    {
                        return; // la boucle ce terminer quand une tache n'est pas terminer
                    }
                }
            } //<- Actualiser les mission
            
        }
    }
    private void SpawnPrefableTache(int i)               
    {
        GameObject New_Object = Instantiate(prefableTache, targetParentPrefable);
        Vector3 Pos = New_Object.transform.position;
        Pos.y = Pos.y - 25 * i;
        New_Object.transform.position = Pos;

        SC_Prefable_Tache Prefable_Script_Tache = New_Object.GetComponentInChildren<SC_Prefable_Tache>();
        //Dans ce code, on vêrifier si la tache en elle même est completer, si oui on change de couleur on rouge puis on le barre
        if (Prefable_Script_Tache != null)
        {
            if (listeMission[i].tacheTerminer)
            {
                Prefable_Script_Tache.textObjectif.color = Color.red;
                Prefable_Script_Tache.textObjectif.text = $"<s>{listeMission[i].tache}</s>";
            }
            else if (!listeMission[i].tacheTerminer)
            {
                Prefable_Script_Tache.textObjectif.color = Color.black;
                Prefable_Script_Tache.textObjectif.text = listeMission[i].tache;
                /*if (Liste_Mission[i].nbrTask + 1 < Liste_Mission[i].nbrTaskMax)
                {
                    Prefable_Script_Tache.Text_Objectif.text = Liste_Mission[i].Tache + $"({}/{})";
                }*/
            }
        }

        listTemporairTache.Add(New_Object);
    }

    public void TerminerTache(string nom_mission)
    {
        for (int i = 0; i < listeMission.Count; i++)
        {
            if (listeMission[i].tacheTerminer == false)
            {
                if(listeMission[i].nomMission == nom_mission)
                {
                    listeMission[i].tacheTerminer = true;
                    ChangeTacheList();
                }
                else
                {
                    Debug.LogWarning($"la mission {listeMission[i].nomMission} estr terminer");
                }
            }
        }
    }

    public void FinEnigme2(int nbrTach)
    {
        if (nbrTach == 0) { nombreTacheValide = nbrTach; }
        else if(nombreTacheValide+1>= nbrTach)
        {
            nombreTacheValide = nbrTach;
            SceneManager.LoadScene("BookShopUpdated");
        }
        else { nombreTacheValide += 1; }

    }

    private void SpawnCanvaGameOver()
    {
        SC_UI_GameOver PUI = prefableCanvaGameOver.GetComponent<SC_UI_GameOver>();
        Transform GO_Canva = GameObject.Find("Canvas").transform;
        if (PUI != null && GO_Canva != null)
        {
            GameObject RR = Instantiate(prefableCanvaGameOver, transform.position, transform.rotation);
            RR.transform.SetParent(GO_Canva, false); // Permet d'annuler

            RR.transform.localScale = new Vector2(0.5f, 0.5f);

            RectTransform rt = RR.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(transform.position.x / 2, transform.position .y/ 2);
        }
    }

    IEnumerator Chronometre()
    {
        _lanceCouroutine = true;

        while (totalSeconds > 0)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            textChronom.text = $"{minutes:D2}:{seconds:D2}";

            yield return new WaitForSeconds(1);

            totalSeconds--;
        }

        textChronom.text = "00:00";
        _lanceCouroutine = false;
        SpawnCanvaGameOver();
    }

    public void SetChronom(bool Continue)
    {
        if(Continue)
        {
            if (!_lanceCouroutine) 
            { 
                StartCoroutine("Chronometre");
                _lanceCouroutine = true;
            }
        }
        else if (!Continue)
        {
            if (_lanceCouroutine)
            {
                StopCoroutine("Chronometre");
                _lanceCouroutine = false;
            }
        }
    }
    #endregion
}
