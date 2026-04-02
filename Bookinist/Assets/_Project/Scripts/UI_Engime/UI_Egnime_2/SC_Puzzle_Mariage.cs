using UnityEngine;

public class SC_Puzzle_Mariage : MonoBehaviour
{
    private SC_Egnime_2 E2;
    private void Start()
    {
        E2 = GameObject.Find("Empty_Egnime2").GetComponent<SC_Egnime_2>();
    }

    public void Take_One_Puzzle() //Fonction permettant a UI interface d'interagire avec l'Egnime 2, Comme c'est un Button Préfable il était Préférable d'utiliser cette mêthode
    {
        E2.Puzzle_Trouver();
        Destroy(gameObject);
    }
}
