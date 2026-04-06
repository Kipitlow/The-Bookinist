/// <summary>
/// Types de conditions disponibles.
/// </summary>
public enum ConditionType
{
    SameLayer,
    SameZone,
    OnTouch,
    IsEmpty,
    IsSameItemSO,
    HasDialogueStarted,
    HasDialogueEnded,
    OnWichFrame,
    HasMoved
}

/// <summary>
/// Types d'actions disponibles.
/// </summary>
public enum ActionType
{
    SetActive,
    Open,
    StartDialogue,
    PlaceObject,
    ClearObject,
    CallFunction,
    CycleSprites,
    Move,
    ResetHasMoved,
    Pick
}