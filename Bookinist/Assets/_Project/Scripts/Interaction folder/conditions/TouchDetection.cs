using UnityEngine;

[RequireComponent(typeof(PlacedObject))]
public class TouchDetection : MonoBehaviour
{
    [Header("Feedback couleur")]
    [SerializeField] private Color _clickColor = Color.red;

    private PlacedObject _placedObject;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private GridInteractor _interactor;
    private InteractionRunner _interactionRunner;

    private void Awake()
    {
        _placedObject = GetComponent<PlacedObject>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;

        _interactionRunner = GetComponent<InteractionRunner>();
    }

    private void Start()
    {
        _interactor = FindObjectOfType<GridInteractor>();

        if (_interactor == null)
        {
            Debug.LogWarning("[ClickableCharacter] Aucun GridInteractor trouvé dans la scène.", this);
            return;
        }

        _interactor.OnObjectTouched += OnObjectTouched;
    }

    private void OnDestroy()
    {
        if (_interactor != null)
            _interactor.OnObjectTouched -= OnObjectTouched;
    }

    private void OnObjectTouched(PlacedObject touched, Vector2Int cell)
    {
        if (touched != _placedObject)
            return;

        _spriteRenderer.color = _clickColor;
        Debug.Log($"[ClickableCharacter] {gameObject.name} a été cliqué !");

        if (_interactionRunner == null)
            return;

        InteractionContext context = new InteractionContext
        {
            instigator = _interactor != null ? _interactor.gameObject : null,
            target = gameObject,
            isTouchEvent = true
        };

        _interactionRunner.TryExecuteAll(context);
    }
}