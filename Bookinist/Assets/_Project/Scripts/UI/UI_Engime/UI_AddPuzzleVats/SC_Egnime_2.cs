using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Énigme 2 : comptabilise puzzles trouvés et notifie SC_Tache.
/// </summary>
public class SC_Egnime_2 : MonoBehaviour
{
    #region Variables

    private int _nbrPuzzle;
    public int nbrPuzzleMax;

    private SC_Tache _scriptTache;
    private string _nameTache;
    private Transform _canvasRect;
    public GameObject prefabUI;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _scriptTache = GameObject.Find("Canvas")?.GetComponent<SC_Tache>();
        _nameTache = "Enigme_2";
        _canvasRect = GameObject.Find("Empty_Cache_Puzzle_Marriage")?.transform;
    }

    #endregion

    #region Methods

    public void PuzzleTrouver()
    {
        if (_scriptTache == null) return;

        if (_nbrPuzzle + 1 >= nbrPuzzleMax)
        {
            foreach (var let in _scriptTache.listeMission)
            {
                if (_nameTache == let.nomMission)
                {
                    let.tache = $"Trouve Tous les Puzzle, sur cette Scene de Mariage ({nbrPuzzleMax}/{nbrPuzzleMax})";
                    _scriptTache.TerminerTache(let.nomMission);
                    break;
                }
            }
        }
        else
        {
            _nbrPuzzle += 1;
            foreach (var let in _scriptTache.listeMission)
            {
                if (_nameTache == let.nomMission)
                {
                    let.tache = $"Trouve Tous les Puzzle, sur cette Scene de Mariage ({_nbrPuzzle}/{nbrPuzzleMax})";
                    _scriptTache.ChangeTacheList();
                    break;
                }
            }
        }
    }

    public void SpawnPuzzle(GameObject supprimerButton)
    {
        if (supprimerButton != null) Destroy(supprimerButton);
        if (prefabUI == null || _canvasRect == null) return;

        for (int i = 0; i < nbrPuzzleMax; i++)
        {
            GameObject obj = Instantiate(prefabUI, _canvasRect);
            RectTransform rect = obj.GetComponent<RectTransform>();
            float x = Random.Range(141, 480);
            float y = Random.Range(146, 1033);
            if (rect != null) rect.anchoredPosition = new Vector2(x, y);
        }
    }

    #endregion
}
