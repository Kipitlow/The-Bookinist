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

    /// <summary>
    /// Appelé par ItemDragHandler.OnEndDrag avec la position écran du doigt.
    /// </summary>
    public void TryDrop(Vector2 screenPosition)
    {
        if (!DragContext.IsDragging) return;

        Item draggedItem = DragContext.DraggedItem;
        Page activePage = _pageManager.GetActivePage();

        if (activePage == null)
        {
            Debug.LogWarning("[WorldDropHandler] Aucune page active trouvée.");
            return;
        }

        Ray ray = _camera.ScreenPointToRay(screenPosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, _raycastDistance);

        InteractionRunner targetRunner = FindRunnerOnActivePage(hits, activePage);

        if (targetRunner != null)
        {
            // Drop réussi : construit le context et déclenche l'interaction
            // instigator = GameObject de l'item UI source (pour traçabilité)
            // target     = objet 3D touché dans le monde
            InteractionContext context = new InteractionContext
            {
                instigator = DragContext.SourceController.gameObject,
                target = targetRunner.gameObject,
                isTouchEvent = false,
                item = draggedItem          // champ à ajouter dans InteractionContext (voir ci-dessous)
            };

            targetRunner.TryExecuteAll(context);
            _inventoryController.RemoveInventoryItem(draggedItem);

            Debug.Log($"[WorldDropHandler] Drop réussi sur '{targetRunner.gameObject.name}' " +
                      $"(page {activePage.PageIndex}) avec '{draggedItem.itemName}'");
        }
        else
        {
            // Aucun objet valide sur la page active -> l'item reste dans l'inventaire
            Debug.Log("[WorldDropHandler] Aucun InteractionRunner sur la page active. " +
                      "L'item retourne dans l'inventaire.");
        }
    }

    /// <summary>
    /// Parcourt les hits et retourne le premier InteractionRunner dont le collider
    /// est listé dans PageObjects de la page active.
    /// Utilise page.PageObjects.Contains() — identique à LayerDetector.IsInSameLayer().
    /// </summary>
    private InteractionRunner FindRunnerOnActivePage(RaycastHit[] hits, Page activePage)
    {
        foreach (var hit in hits)
        {
            GameObject hitObj = hit.collider.gameObject;

            // Même logique que LayerDetector.IsInSameLayer()
            if (!activePage.PageObjects.Contains(hitObj))
                continue;

            InteractionRunner runner = hitObj.GetComponent<InteractionRunner>();
            if (runner == null)
                runner = hitObj.GetComponentInParent<InteractionRunner>();

            if (runner != null)
                return runner;
        }

        return null;
    }
}