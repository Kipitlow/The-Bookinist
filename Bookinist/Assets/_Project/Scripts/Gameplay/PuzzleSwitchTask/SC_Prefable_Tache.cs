using TMPro;
using UnityEngine;

public class SC_Prefable_Tache : MonoBehaviour
{
    public TextMeshProUGUI textObjectif;
    public TextMeshProUGUI textRécompence;
    public void LigneBarrer()
    {
        string Text = textObjectif.text;
        textObjectif.text = $"<s>{Text}</s>";
    }
}
