using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [TextArea(2, 4)]
    public string[] lines;
}