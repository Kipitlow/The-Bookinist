using UnityEngine;

public class SC_Egnime_2 : MonoBehaviour
{
    [Header("Nombre de Puzzle")]
    private int Nbr_Puzzle;
    [SerializeField] public int Nbr_Puzzle_Max;

    [Header("Gestion UI")]
    private SC_Tache Script_Tache;
    private string NameTach;
    private Transform canvasRect;
    public GameObject prefabUI;


    void Start()
    {
        Script_Tache = GameObject.Find("Canvas").GetComponent<SC_Tache>();
        NameTach = "Enigme_2";

        canvasRect = GameObject.Find("Empty_Cache_Puzzle_Marriage").GetComponent<Transform>();
    }

    public void Puzzle_Trouver()
    {
        {
            /*if (Nbr_Puzzle + 1 >= Nbr_Puzzle_Max) // Condition de reussit du puzzle
            {
                if (Script_Tache != null)
                {
                    foreach (Tache_Layourt TL in Script_Tache.Tache_Dans_Ce_Layeur)
                    {
                        foreach (List_Element_Tach LET in TL.list_Element_Taches)
                        {
                            if (NameTach == LET.Nom_Mission)
                            {
                                LET.TacheTerminer = true;
                                LET.Tache = $"Trouve Tous les Puzzle, sur cette Scene de Mariage ({Nbr_Puzzle_Max}/{Nbr_Puzzle_Max})";
                                Script_Tache.Change_Tach_List();
                                //ICI que la mission ce terminer, est donc de récompencer c'est joueur.
                            }
                        }
                    }
                }
            }
            else
            {
                Nbr_Puzzle += 1;
                if (Script_Tache != null)
                {
                    foreach (Tache_Layourt TL in Script_Tache.Tache_Dans_Ce_Layeur)
                    {
                        foreach (List_Element_Tach LET in TL.list_Element_Taches)
                        {
                            if (NameTach == LET.Nom_Mission)
                            {
                                LET.Tache = $"Trouve Tous les Puzzle, sur cette Scene de Mariage ({Nbr_Puzzle}/{Nbr_Puzzle_Max})";
                                Script_Tache.Change_Tach_List();
                                //ICI que la mission ce terminer, est donc de récompencer c'est joueur.
                            }
                        }
                    }
                }
            }*/
        }
        if (Nbr_Puzzle + 1 >= Nbr_Puzzle_Max) // Condition de reussit du puzzle
        {
            if (Script_Tache != null)
            {
                foreach (List_Element_Tach LET in Script_Tache.listeMission)
                {
                    if (NameTach == LET.nomMission)
                    {
                        LET.tache = $"Trouve Tous les Puzzle, sur cette Scene de Mariage ({Nbr_Puzzle_Max}/{Nbr_Puzzle_Max})";
                        Script_Tache.TerminerTache(LET.nomMission);
                        //Script_Tache.Change_Tach_List();
                    }
                }
            }
        }
        else
        {
            Nbr_Puzzle += 1;
            if (Script_Tache != null)
            {
                foreach (List_Element_Tach LET in Script_Tache.listeMission)
                {
                    if (NameTach == LET.nomMission)
                    {
                        LET.tache = $"Trouve Tous les Puzzle, sur cette Scene de Mariage ({Nbr_Puzzle}/{Nbr_Puzzle_Max})";
                        Script_Tache.ChangeTacheList();
                        //ICI que la mission ce terminer, est donc de récompencer c'est joueur.
                    }
                }
            }
        }
    }


    public void Spawn_Puzzle(GameObject Supprimer_Button)
    {
        Destroy(Supprimer_Button);
        for (int i = 0; i < Nbr_Puzzle_Max; i++)
        {
            GameObject obj = Instantiate(prefabUI, canvasRect);

            RectTransform rect = obj.GetComponent<RectTransform>();

            // Position aléatoire dans le Canvas
            float x = Random.Range(141, 480);
            float y = Random.Range(146, 1033);

            rect.anchoredPosition = new Vector2(x, y);
        }
    }
}
