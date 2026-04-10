using UnityEngine;

/// <summary>
/// Singleton placé sur un Manager GameObject (ex: GameManager).
/// Reçoit la position écran de fin de drag, lance un Raycast 3D
/// filtré par la page active, et déclenche l'InteractionRunner si touché.
/// </summary>
public class WorldDropHandler : MonoBehaviour
{
    public static WorldDropHandler Instance { get; private set; }

    [Header("Références")]
    [Tooltip("Référence au PageManager pour connaître la page active.")]
    [SerializeField] private PageManager _pageManager;

    [Tooltip("Référence à l'InventoryController pour retirer l'item si drop réussi.")]
    [SerializeField] private InventoryController _inventoryController;

    [Tooltip("Caméra utilisée pour le raycast. Si null, Camera.main est utilisée.")]
    [SerializeField] private Camera _camera;

    [Header("Paramètres")]
    [Tooltip("Distance max du raycast depuis la caméra.")]
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

    private void LateUpdate()
    {
        DragContext.ConsumeDropFlag();
    }

    /// <summary>
    /// Appelé par ItemDragHandler.OnEndDrag avec la position écran du doigt.
    /// </summary>
    public void TryDrop(Vector2 screenPosition)
    {
        if (!DragContext.IsDragging) return;

        Item draggedItem = DragContext.DraggedItem;

        Debug.Log($"[Drop] TryDrop appelé. IsDragging={DragContext.IsDragging}, screenPos={screenPosition}");

        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Physics.Raycast(ray, out RaycastHit hit, _raycastDistance);

        if (hit.collider == null) return;

        //InteractionRunner targetRunner = FindRunnerOnActivePage(hits, activePage);
        InteractionRunner targetRunner = hit.collider.gameObject.GetComponent<InteractionRunner>();

        int hitlayer = hit.collider.GetComponentInParent<Page>().PageIndex;
        int camLayer = _camera.GetComponent<CameraMovement>().currentIndexLayer;

        if (targetRunner != null && hitlayer == camLayer)
        {
            // Drop réussi : construit le context et déclenche l'interaction
            // instigator = GameObject de l'item UI source (pour traçabilité)
            // target     = objet 3D touché dans le monde
            InteractionContext context = new InteractionContext
            {
                instigator = DragContext.SourceController.gameObject,
                target = targetRunner.gameObject,
                isTouchEvent = false,
                item = draggedItem
            };

            bool wasHandled = targetRunner.TryExecuteAll(context);
            if (wasHandled)
                _inventoryController.RemoveInventoryItem(draggedItem);

        }
        else
        {
            // Aucun objet valide sur la page active -> l'item reste dans l'inventaire
            Debug.Log("[WorldDropHandler] Aucun InteractionRunner sur la page active. " +
                      "L'item retourne dans l'inventaire.");
        }
    }
}