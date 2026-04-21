public enum ConditionType
{
    ISBookFinish,
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
    CanBePlacedInBalance,
    WeightIsMoreThan,
    WeightIsLessThan,
}

public enum ActionType
{
    FinishBook,
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
    PlaceInBalance,
    FeedBack,
    CloseDialogue,
}