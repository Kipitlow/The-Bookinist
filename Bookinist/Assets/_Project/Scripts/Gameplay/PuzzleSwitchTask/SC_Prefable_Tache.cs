using TMPro;
using UnityEngine;

public class SC_Prefable_Tache : MonoBehaviour
{
    #region Variables

    public TextMeshProUGUI textObjectif;
    public TextMeshProUGUI textRecompence;

    #endregion

    #region Methods

    public void LigneBarrer()
    {
        if (textObjectif == null) return;
        string text = textObjectif.text;
        textObjectif.text = $"<s>{text}</s>";
    }

    #endregion
}
