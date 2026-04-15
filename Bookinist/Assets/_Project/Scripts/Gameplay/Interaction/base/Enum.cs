public enum ConditionType
{
    SameLayer,
    SameZone,
    OnTouch,
    IsEmpty,
    IsSameItemSO,
    IsNotSameItemSO,
    HasDialogueStarted,
    HasDialogueEnded,
    OnWichFrame,
    HasMoved,
    HasToCheckEmptynessInventory,
    CanBePlacedInBalance
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
    Pick,
    Destroy,
    Drop,
    PlaceInBalance
}