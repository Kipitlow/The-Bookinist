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
    HasMoved,
    HasToCheckEmptynessInventory
}

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