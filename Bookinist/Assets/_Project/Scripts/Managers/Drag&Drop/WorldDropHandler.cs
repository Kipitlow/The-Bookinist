using JetBrains.Annotations;
using System;
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

    private int _camLayer;
    private int _hitlayer;

    public event Action OnDropItem;

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

        bool shouldDrop = true;
        Item draggedItem = DragContext.DraggedItem;

        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Physics.Raycast(ray, out RaycastHit hit, _raycastDistance);

        _camLayer = _camera.GetComponent<CameraMovement>().currentIndexLayer;

        //place Object In World
        if (hit.collider == null)
        {
            DropObject(screenPosition);
            return;
        };

        InteractionRunner targetRunner = hit.collider.gameObject.GetComponent<InteractionRunner>();

        _hitlayer = hit.collider.GetComponentInParent<Page>().PageIndex;

        if (_hitlayer == _camLayer)
        {
            if (hit.collider.tag.Equals("LowCollider"))
                shouldDrop = false;
        }

        if (targetRunner != null && _hitlayer == _camLayer)
        {
            InteractionContext context = new InteractionContext
            {
                instigator = DragContext.SourceController.gameObject,
                target = targetRunner.gameObject,
                isTouchEvent = false,
                item = draggedItem
            };

            bool wasHandled = targetRunner.TryExecuteAll(context);
            if (wasHandled)
            {
                _inventoryController.RemoveInventoryItem(draggedItem);
                shouldDrop = false;
            }
        }
        if(shouldDrop) 
            DropObject(screenPosition);
    }
    public void DropObject(Vector2 screenPosition)
    {
        Item draggedItem = DragContext.DraggedItem;

        Transform activeLayer = _pageManager.GetPageFromInt(_camLayer).transform;
        Page page = activeLayer.GetComponent<Page>();

        float depth = activeLayer.position.z - _camera.transform.position.z;

        Vector3 screenPoint = new Vector3(screenPosition.x, screenPosition.y, depth);
        Vector3 worldPoint = _camera.ScreenToWorldPoint(screenPoint);

        GameObject droppedObject = Instantiate(_prefabDropableObject, worldPoint, _prefabDropableObject.transform.rotation, activeLayer);
        BoxCollider boxCollider = droppedObject.GetComponent<BoxCollider>();

        //setup SpriteRenderer
        SpriteRenderer spriteRenderer = droppedObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = draggedItem.itemSprite;
        spriteRenderer.sortingLayerName = "Page_" + _camLayer;
        spriteRenderer.sortingOrder = page.PageObjects.Count;

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector3 size = droppedObject.transform.localScale;


        Vector3 worldSpriteSize = spriteRenderer.bounds.size;
        Vector3 lossyScale = droppedObject.transform.lossyScale;

        Vector3 scale = droppedObject.transform.localScale;
        scale.x = size.x / spriteSize.x;
        scale.y = size.y / spriteSize.y;

        droppedObject.transform.localScale = scale;

        boxCollider.size = new Vector3(
            worldSpriteSize.x / lossyScale.x,
            worldSpriteSize.y / lossyScale.y,
            boxCollider.size.z
         );

        Vector3 localCenter = spriteRenderer.sprite.bounds.center;

        boxCollider.center = new Vector3(
            localCenter.x,
            localCenter.y,
            boxCollider.center.z
        );
        //Set MoveOnZoom
        droppedObject.GetComponent<MoveOnZoom>().SetIndexs(_camLayer, _camera.GetComponent<CameraMovement>().currentIndexByLayer);

        //set Pickable
        droppedObject.GetComponent<Pickable>().SetItem(draggedItem);

        _inventoryController.RemoveInventoryItem(draggedItem);

        OnDropItem?.Invoke();
        return;
    }
}