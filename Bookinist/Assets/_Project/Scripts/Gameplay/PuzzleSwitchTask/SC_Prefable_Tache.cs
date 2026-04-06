using TMPro;
using UnityEngine;

public class SC_Prefable_Tache : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI Text_Objectif;
    [SerializeField] public TextMeshProUGUI Text_Récompence;
    public void ligne_Barrer()
    {
        string Text = Text_Objectif.text;
        Text_Objectif.text = $"<s>{Text}</s>";
    }
}
