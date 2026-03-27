using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_NomPNJ", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Serializable]
    public class DialogueLine
    {
        [TextArea(2, 5)]
        public string text;

        [Tooltip("Nom du PNJ")]
        public string speakerOverride;
    }

    [Header("Lignes du dialogue (dans l'ordre)")]
    public List<DialogueLine> lines = new();
}