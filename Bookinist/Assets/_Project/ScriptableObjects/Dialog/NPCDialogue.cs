using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("Dialogue Info")]
    public string NPCName;
    [TextArea(2, 4)]
    public string[] lines;

    [Header("Dialogue Type")]
    public bool IsShopNPC = false;
    public bool isLoopable = false;
    public int timesEnded = 0;
}