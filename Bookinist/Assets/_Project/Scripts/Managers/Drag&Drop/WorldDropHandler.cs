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

        Debug.Log($"[Drop] TryDrop appelé. IsDragging={DragContext.IsDragging}, screenPos={screenPosition}");

        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Physics.Raycast(ray, out RaycastHit hit, _raycastDistance);

        Debug.Log($"[Drop] -> {hit.collider.gameObject.name}");

        //InteractionRunner targetRunner = FindRunnerOnActivePage(hits, activePage);
        InteractionRunner targetRunner = hit.collider.gameObject.GetComponent<InteractionRunner>();

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
    /// Utilise page.PageObjects.Contains() - identique à LayerDetector.IsInSameLayer().
    /// </summary>
    private InteractionRunner FindRunnerOnActivePage(RaycastHit[] hits, Page activePage)
    {
        foreach (var hit in hits)
        {
            // 1. Remonte depuis le hit pour trouver l'empty listé dans pageObjects
            GameObject pageObj = GetPageObject(hit.collider.gameObject, activePage);
            if (pageObj == null) continue;

            // 2. L'InteractionRunner peut être sur le hit lui-même,
            //    un parent, ou un enfant du pageObj
            InteractionRunner runner = hit.collider.GetComponent<InteractionRunner>();
            if (runner == null)
                runner = hit.collider.GetComponentInParent<InteractionRunner>();
            if (runner == null)
                runner = pageObj.GetComponentInChildren<InteractionRunner>();

            if (runner != null)
                return runner;
        }
        return null;
    }

    private GameObject GetPageObject(GameObject hitObj, Page activePage)
    {
        Transform current = hitObj.transform;
        while (current != null)
        {
            if (activePage.PageObjects.Contains(current.gameObject))
                return current.gameObject;
            current = current.parent;
        }
        return null;
    }
}