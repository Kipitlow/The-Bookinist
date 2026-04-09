using UnityEngine;

/// <summary>
/// Singleton plac� sur un Manager GameObject (ex: GameManager).
/// Re�oit la position �cran de fin de drag, lance un Raycast 3D
/// filtr� par la page active, et d�clenche l'InteractionRunner si touch�.
/// </summary>
public class WorldDropHandler : MonoBehaviour
{
    public static WorldDropHandler Instance { get; private set; }

    [Header("R�f�rences")]
    [Tooltip("R�f�rence au PageManager pour conna�tre la page active.")]
    [SerializeField] private PageManager _pageManager;

    [Tooltip("R�f�rence � l'InventoryController pour retirer l'item si drop r�ussi.")]
    [SerializeField] private InventoryController _inventoryController;

    [Tooltip("Cam�ra utilis�e pour le raycast. Si null, Camera.main est utilis�e.")]
    [SerializeField] private Camera _camera;

    [Header("Param�tres")]
    [Tooltip("Distance max du raycast depuis la cam�ra.")]
    [SerializeField] private float _raycastDistance = 200f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    /// <summary>
    /// Appel� par ItemDragHandler.OnEndDrag avec la position �cran du doigt.
    /// </summary>
    public void TryDrop(Vector2 screenPosition)
    {
        if (!DragContext.IsDragging) return;

        Item draggedItem = DragContext.DraggedItem;
        Page activePage = _pageManager.GetActivePage();

        if (activePage == null)
        {
            return;
        }

        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Physics.Raycast(ray, out RaycastHit hit, _raycastDistance);

        InteractionRunner targetRunner = hit.collider.gameObject.GetComponent<InteractionRunner>();

        int hitlayer = hit.collider.GetComponentInParent<Page>().PageIndex;
        int camLayer = _camera.GetComponent<CameraMovement>().currentIndexLayer;

        if (targetRunner != null && hitlayer == camLayer)
        {
            // Drop r�ussi : construit le context et d�clenche l'interaction
            // instigator = GameObject de l'item UI source (pour tra�abilit�)
            // target     = objet 3D touch� dans le monde

            InteractionContext context = new InteractionContext
            {
                instigator = DragContext.SourceController.gameObject,
                target = targetRunner.gameObject,
                item = draggedItem          // champ � ajouter dans InteractionContext (voir ci-dessous)
            };

            targetRunner.TryExecuteAll(context);
        }
    }

    public void Drop(Item item, Slot slot)
    {
        slot.FillWithSprite(item);
        _inventoryController.RemoveInventoryItem(item);
    }
}