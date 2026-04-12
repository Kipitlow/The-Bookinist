using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// À placer sur le même GameObject que ItemController.
/// Gère le drag visuel (ghost qui suit le doigt) et délègue
/// la logique de drop au WorldDropHandler.
/// </summary>
[RequireComponent(typeof(ItemController))]
public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("Le prefab du ghost (simple Image avec le sprite de l'item). " +
             "Si null, un ghost est créé dynamiquement depuis l'image de cet item.")]
    [SerializeField] private GameObject _ghostPrefab;

    //Composants frères 
    private ItemController _itemController;
    private Image _itemImage;

    //Ghost runtime
    private GameObject _ghost;
    private RectTransform _ghostRect;

    // Divers
    private CanvasGroup _canvasGroup;
    private Canvas _rootCanvas;

    #region Unity Methods

    private void Awake()
    {
        _itemController = GetComponent<ItemController>();
        _itemImage = GetComponentInChildren<Image>();

        _rootCanvas = FindAnyObjectByType<Canvas>();

        // CanvasGroup optionnel : permet de passer en semi-transparent pendant le drag
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    #endregion

    #region Drag Handlers

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_rootCanvas == null)
        {
            Debug.LogError("[ItemDragHandler] _rootCanvas non assigné !", this);
            return;
        }

        // Stocke l'item dans le contexte partagé
        DragContext.BeginDrag(_itemController.ItemScriptable, _itemController);

        _itemImage.enabled = false;

        // Crée le ghost
        _ghost = CreateGhost();

        // Rend l'item source semi-transparent pour indiquer qu'il est "en l'air"
        if (_canvasGroup != null)
            _canvasGroup.alpha = 0.4f;

        // Empêche le scroll de l'inventaire de capturer le drag
        eventData.pointerDrag = this.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_ghost == null) return;

        // Convertit la position écran en position locale dans le Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rootCanvas.transform as RectTransform,
            eventData.position,
            _rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _rootCanvas.worldCamera,
            out Vector2 localPoint
        );

        _ghostRect.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restaure l'opacité
        if (_canvasGroup != null)
            _canvasGroup.alpha = 1f;

        _itemImage.enabled = true;

        // Détruit le ghost
        if (_ghost != null)
        {
            Destroy(_ghost);
            _ghost = null;
        }

        // Tente le drop dans le monde
        if (WorldDropHandler.Instance != null)
            WorldDropHandler.Instance.TryDrop(eventData.position);

        

        // Nettoie le contexte
        DragContext.EndDrag();
    }

    #endregion

    #region Ghost

    private GameObject CreateGhost()
    {
        GameObject ghost;

        if (_ghostPrefab != null)
        {
            ghost = Instantiate(_ghostPrefab, _rootCanvas.transform);
        }
        else
        {
            // Crée un ghost minimal : même sprite que l'item, même taille
            ghost = new GameObject("DragGhost", typeof(RectTransform), typeof(Image));
            ghost.transform.SetParent(_rootCanvas.transform, false);

            Image ghostImage = ghost.GetComponent<Image>();
            if (_itemImage != null)
            {
                ghostImage.sprite = _itemImage.sprite;
                //ghostImage.SetNativeSize();
            }
        }

        // Met le ghost tout en haut du Canvas (au-dessus de tout)
        ghost.transform.SetAsLastSibling();

        // Désactive le raycast pour que le ghost ne bloque pas les EventSystems
        Image img = ghost.GetComponent<Image>();
        if (img != null) img.raycastTarget = false;

        _ghostRect = ghost.GetComponent<RectTransform>();
        // Ancre au centre pour que le ghost soit centré sous le doigt
        _ghostRect.pivot = new Vector2(0.5f, 0.5f);

        return ghost;
    }

    #endregion
}