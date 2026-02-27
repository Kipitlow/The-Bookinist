using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InteractableChest : MonoBehaviour, IInteractable
{
    [Header("Sprites")]
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;

    SpriteRenderer spriteRenderer;
    bool isOpen = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite;
    }

    public void OnTap(Vector3 hitPoint)
    {
        isOpen = !isOpen;
        if (isOpen)
            spriteRenderer.sprite = openSprite;
        else 
            spriteRenderer.sprite = closedSprite;
    }
}
