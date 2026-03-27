using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Données")]
    [SerializeField] private DialogueData _dialogue;
    [SerializeField] private string _speakerName;

    [Header("Références")]
    [SerializeField] private DialogueBubble _bubble;

    [Header("Interaction")]
    [Tooltip("Rayon de détection du tap autour du PNJ (en unités monde).")]
    [SerializeField] private float _tapRadius = 0.8f;

    // ── État ──────────────────────────────────────────────────
    private int _currentLine = -1; // -1 = dialogue fermé
    private bool _isActive => _currentLine >= 0;

    // ── Input ─────────────────────────────────────────────────
    private InputAction _tapAction;
    private InputAction _tapPositionAction;
    private Camera _cam;

    // ──────────────────────────────────────────────────────────
    //  Unity lifecycle
    // ──────────────────────────────────────────────────────────

    private void Awake()
    {
        _cam = Camera.main;
        CreateInputActions();
    }

    private void OnEnable()
    {
        _tapAction.performed += OnTap;
        _tapAction.Enable();
        _tapPositionAction.Enable();
    }

    private void OnDisable()
    {
        _tapAction.performed -= OnTap;
        _tapAction.Disable();
        _tapPositionAction.Disable();
    }

    private void OnDestroy()
    {
        _tapAction?.Dispose();
        _tapPositionAction?.Dispose();
    }

    // ──────────────────────────────────────────────────────────
    //  Input
    // ──────────────────────────────────────────────────────────

    private void CreateInputActions()
    {
        _tapAction = new InputAction("Tap", InputActionType.Button);
        _tapAction.AddBinding("<Touchscreen>/primaryTouch/press");
        _tapAction.AddBinding("<Mouse>/leftButton");

        _tapPositionAction = new InputAction("TapPosition",
            InputActionType.Value, expectedControlType: "Vector2");
        _tapPositionAction.AddBinding("<Touchscreen>/primaryTouch/position");
        _tapPositionAction.AddBinding("<Mouse>/position");
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        if (_dialogue == null || _bubble == null) return;

        Vector2 screenPos = _tapPositionAction.ReadValue<Vector2>();
        Vector3 worldPos = _cam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, _cam.nearClipPlane));

        // Ignore le tap si la position n'est pas proche du PNJ
        // ET que le dialogue n'est pas déjà ouvert
        if (!_isActive)
        {
            float dist = Vector2.Distance(worldPos, transform.position);
            if (dist > _tapRadius) return;
        }

        Advance();
    }

    // ──────────────────────────────────────────────────────────
    //  Logique de dialogue
    // ──────────────────────────────────────────────────────────

    /// <summary>
    /// Avance d'une ligne. Si on est à la fin, ferme le dialogue.
    /// Si le dialogue est fermé, l'ouvre depuis la première ligne.
    /// </summary>
    private void Advance()
    {
        if (_dialogue.lines.Count == 0) return;

        _currentLine++;

        if (_currentLine >= _dialogue.lines.Count)
        {
            Close();
            return;
        }

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        DialogueData.DialogueLine line = _dialogue.lines[_currentLine];

        // Utilise l'override si renseigné, sinon le nom par défaut du PNJ
        string speaker = string.IsNullOrEmpty(line.speakerOverride)
            ? _speakerName
            : line.speakerOverride;

        // Format : "Nom du personnage\nTexte" si un nom est défini
        string display = string.IsNullOrEmpty(speaker)
            ? line.text
            : $"<b>{speaker}</b>\n{line.text}";

        _bubble.Show(display);
    }

    private void Close()
    {
        _currentLine = -1;
        _bubble.Hide();
    }

    // ──────────────────────────────────────────────────────────
    //  Gizmo de debug
    // ──────────────────────────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _tapRadius);
    }
}