using UnityEngine;

/// <summary>
/// Simple utilitaire pour afficher du texte (Debug.Log).
/// </summary>
public class PrintText : MonoBehaviour
{
    #region Methods

    public void Print(string text)
    {
        Debug.Log(text);
    }

    #endregion
}
