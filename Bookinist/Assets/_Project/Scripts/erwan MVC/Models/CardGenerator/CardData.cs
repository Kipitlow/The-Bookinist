using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Identification")]
    public string cardId;   // Unique id if needed for save/lookup.
    public string cardName; // Display name.

    [Tooltip("Sprite of the card front.")]
    public Sprite cardtexture;

    [Header("Game info")]
    public string description; // Optional description or lore.
    
    [Header("Power values (Double Twin / Evoland style)")]
    public int powerLeft;   // Value on the left side of the card.
    public int powerTop;    // Value on the top side of the card.
    public int powerRight;  // Value on the right side of the card.
    public int powerBottom; // Value on the bottom side of the card.
}
