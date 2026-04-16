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

    [Tooltip("Référence au PageManager pour connaître la page active.")]
    [SerializeField] private GameObject _prefabDropableObject;

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

        int camLayer = _camera.GetComponent<CameraMovement>().currentIndexLayer;

        //place Object In World
        if (hit.collider == null)
        {
            Debug.Log("tried to spawn object");

            Transform activeLayer = _pageManager.GetPageFromInt(camLayer).transform;
            Page page = activeLayer.GetComponent<Page>();

            float depth = activeLayer.position.z - _camera.transform.position.z;

            Vector3 screenPoint = new Vector3(screenPosition.x, screenPosition.y, depth);
            Vector3 worldPoint = _camera.ScreenToWorldPoint(screenPoint);

            GameObject droppedObject = Instantiate(_prefabDropableObject, worldPoint, _prefabDropableObject.transform.rotation, activeLayer);

            //setup SpriteRenderer
            SpriteRenderer spriterenderer = droppedObject.GetComponent<SpriteRenderer>();
            spriterenderer.sprite = draggedItem.itemSprite;
            spriterenderer.sortingLayerName = "Page_" + camLayer;
            spriterenderer.sortingOrder = page.PageObjects.Count;

            Vector2 spriteSize = spriterenderer.sprite.bounds.size;
            Vector3 size = droppedObject.transform.localScale;

            Vector3 scale = droppedObject.transform.localScale;
            scale.x = size.x / spriteSize.x;
            scale.y = size.y / spriteSize.y;

            droppedObject.transform.localScale = scale;

            //Set MoveOnZoom
            droppedObject.GetComponent<MoveOnZoom>().SetIndexs(camLayer, _camera.GetComponent<CameraMovement>().currentIndexByLayer);

            //set Pickable
            droppedObject.GetComponent<Pickable>().SetItem(draggedItem);

            _inventoryController.RemoveInventoryItem(draggedItem);

            return;
        };

        InteractionRunner targetRunner = hit.collider.gameObject.GetComponent<InteractionRunner>();

        int hitlayer = hit.collider.GetComponentInParent<Page>().PageIndex;

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
    }
}