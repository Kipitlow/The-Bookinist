using System;
using System.Collections;
using System.Collections.Generic; // Permet de faire un tableau
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class List_Element_Tach
{
    [SerializeField] public string Nom_Mission;
    [SerializeField] public string Tache;
    [SerializeField] public bool TacheTerminer;
}

public class SC_Tache : MonoBehaviour
{
    #region Variable
    [Header("Variable Utiliser pour le Chronometre")]
    [SerializeField] public TextMeshProUGUI Text_Chronom;
    private bool lanceCouroutine;
    //[SerializeField] public TextMeshProUGUI Text_Objectif; //////Objectif
    public int totalSeconds;

    /*[Header("Autre")]
    public Camera CM_Player;
    [SerializeField]public CameraMovement CM;*/
    //private int Layeur_Actuelle_Du_Joueur;

    [Header("UI_Enigme_02")]
    public int NombreTacheValide = 0;

    [Header("Prefable")]
    [SerializeField] public GameObject PrefableTache;
    [SerializeField] public GameObject Prefable_Canva_GameOver;
    [SerializeField] public Transform Target_Parent_Prefable;
    public List<GameObject> List_Temporair_Tache = new List<GameObject>(); //Permet de stocker les Prefable_Tache.

    [Header("Mission")]
    public List<List_Element_Tach> Liste_Mission = new List<List_Element_Tach>(); //Permet de stocker les Prefable_Tache.

    [Header("Hint System")]
    [SerializeField] private GameObject _hintsPanel;
    [SerializeField] private GameObject _hintsButton;
    [SerializeField] private TextMeshProUGUI _hintNumberTextMesh;

    private bool _isAlreadyOpenedPanel = false;

    #endregion

    #region Unity Methods
    void Start()
    {
        //if (CM_Player == null) CM_Player = GameObject.Find("CameraManager").GetComponent<Camera>();

        if (SceneManager.GetActiveScene().name != "Enigme1")
            StartCoroutine("Chronometre"); //Permet de lancer la coroutine;

        _hintNumberTextMesh.text = GameManager.Instance.GetHintNumber().ToString();
        SetupTache();
    }
    #endregion

    #region Methods

    public void Change_Tach_List()//CodePermettant de actualiser les objectif du joueur
    {
        //On fonction quand terminer la tache précédent on veut switch de tache.
        if (PrefableTache != null && Target_Parent_Prefable != null)
        {
            StartCoroutine(Test());
        }
    }

    IEnumerator Test()
    {
        // Code qui permet de supprimer tout préfable tache dans le code 
        foreach (GameObject obj in List_Temporair_Tache)
        {
            if (obj != null)
            {
                StartCoroutine(WaitForCrossList(obj));
            }
        }

        yield return new WaitForSeconds(2.5f);



        //List_Temporair_Tache.Clear();
        //Code permet d'afficher tous les mission terminer et une seul mission non terminer
        SetupTache();

        _isAlreadyOpenedPanel = false;
    }

    private void SetupTache()
    {
        for (int i = 0; i < Liste_Mission.Count; i++)
        {
            if (Liste_Mission[i].TacheTerminer == false)
            {
                Spawn_Prefable_Tache(i);
                break; // la boucle ce terminer quand une tache n'est pas terminer
            }
        }
    }

    private void Spawn_Prefable_Tache(int i)
    {
        GameObject New_Object = Instantiate(PrefableTache, Target_Parent_Prefable);
        //Vector3 Pos = New_Object.transform.position;
        //Pos.y = Pos.y - 25 * i;
        //New_Object.transform.position = Pos;

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
                if (Liste_Mission[i].Nom_Mission == nom_mission)
                {
                    Liste_Mission[i].TacheTerminer = true;
                    //Liste_Mission[i];
                    Change_Tach_List();
                }
                else
                {
                    //Debug.LogWarning($"la mission {Liste_Mission[i].Nom_Mission} estr terminer");
                }
            }
        }
    }

    public void UseHintWrapper()
    {
        if (_isAlreadyOpenedPanel == false)
        {
            if (GameManager.Instance.UseHint() == false) return;
        }

        _hintNumberTextMesh.text = GameManager.Instance.GetHintNumber().ToString();
        _hintsButton.SetActive(false);
        _hintsPanel.SetActive(true);

        _isAlreadyOpenedPanel = true;
    }

    public void FinEnigme2(int nbrTach)
    {
        if (nbrTach == 0) { NombreTacheValide = nbrTach; }
        else if (NombreTacheValide + 1 >= nbrTach)
        {
            NombreTacheValide = nbrTach;
            SceneManager.LoadScene("BookShopUpdated");
        }
        else { NombreTacheValide += 1; }

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
            rt.anchoredPosition = new Vector2(transform.position.x / 2, transform.position.y / 2);
        }
    }

    IEnumerator Chronometre()
    {
        lanceCouroutine = true;

        while (totalSeconds > 0)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            Text_Chronom.text = $"{minutes:D2}:{seconds:D2}";

            yield return new WaitForSeconds(1);

            totalSeconds--;
        }

        Text_Chronom.text = "00:00";
        lanceCouroutine = false;
        Spawn_Canva_GameOver();
    }

    IEnumerator WaitForCrossList(GameObject obj)
    {
        obj.GetComponent<SC_Prefable_Tache>().ligne_Barrer();

        yield return new WaitForSeconds(2);
        Destroy(obj);

        _hintsPanel.SetActive(false);
        _hintsButton.SetActive(true);

    }

    public void SetChronom(bool Continue)
    {
        if (Continue)
        {
            if (!lanceCouroutine)
            {
                StartCoroutine("Chronometre");
                lanceCouroutine = true;
            }
        }
        else if (!Continue)
        {
            if (lanceCouroutine)
            {
                StopCoroutine("Chronometre");
                lanceCouroutine = false;
            }
        }
    }
    #endregion
}
