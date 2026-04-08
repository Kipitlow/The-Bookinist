using UnityEngine;

/// <summary>
/// Interface UI pour prendre un puzzle (détruit son GameObject et notifie SC_Egnime_2).
/// </summary>
public class SC_Puzzle_Mariage : MonoBehaviour
{
    private SC_Egnime_2 _e2;

    private void Start()
    {
        _e2 = GameObject.Find("@Egnime2: Puzzle").GetComponent<SC_Egnime_2>();
    }

    public void TakeOnePuzzle()
    {
        _e2.PuzzleTrouver();
        Destroy(gameObject);
    }
}
