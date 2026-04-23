using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRunner : MonoBehaviour
{
    [SerializeField] private List<InteractionSet> _interactionSets = new();
    private InteractionFeedBack _interactionFeedBack;
    private bool _conditionWasTrue;

    private void Awake()
    {
        _interactionFeedBack = GetComponent<InteractionFeedBack>();
    }


    #region Try
    public bool TryExecuteAll(InteractionContext context)
    {
        bool anyExecuted = false;
        foreach (var set in _interactionSets)
        {
            if (AreConditionsValid(set.conditions, context))
            {
                ExecuteActions(set.actions, context);
                anyExecuted = true;
            }
        }
        return anyExecuted;
    }

    private bool AreConditionsValid(List<ConditionEntry> conditions, InteractionContext context)
    {
        foreach (var condition in conditions)
        {
            if (!EvaluateCondition(condition, context))
                return false;
        }

        return true;
    }

    private void ExecuteActions(List<ActionEntry> actions, InteractionContext context)
    {
        foreach (var action in actions)
        {
            ExecuteAction(action, context);
        }
    }
    #endregion

    #region test Condition

    private bool EvaluateCondition(ConditionEntry condition, InteractionContext context)
    {
        switch (condition.type)
        {
            case ConditionType.ISBookFinish:
                return GameManager.Instance.bookFinish == condition.isBookFinish;


            case ConditionType.SameLayer:
                if (condition.target == null)
                    return false;

                return condition.layerDetector.IsInSameLayer(condition.target, condition.checkedPage);

            case ConditionType.SameZone:
                if (condition.zone == null || condition.target == null)
                    return false;

                return context.target == condition.target;

            case ConditionType.OnTouch:
                if (context.target == null || !context.isTouchEvent) return false;
                return this.gameObject == context.target;

            case ConditionType.IsEmpty:
                if (condition.slot == null)
                    return false;
                return condition.slot.IsEmpty() == condition.shouldBeEmpty;

            case ConditionType.IsSameItemSO:
                if (condition.item == null)
                    return false;
                return condition.selectedItemIsWanted.IsCorrectObject(condition.item);

            case ConditionType.IsNotSameItemSO:
                if (condition.item == null)
                    return false;
                return !condition.selectedItemIsWanted.IsCorrectObject(condition.item);
                
            case ConditionType.OnWichFrame:
                if (condition.cycleThroughSprite == null)
                    return false;
                return condition.cycleThroughSprite.IsAtThisFrame(condition.WantedFrame, condition.trueIfMore);

            case ConditionType.HasDialogueStarted:
                if (condition.npcTalker == null)
                    return false;
                return condition.npcTalker._lineIndex >= 1;

            case ConditionType.HasDialogueEnded:
                if (condition.npcTalker == null)
                    return false;
                return condition.npcTalker._hasDialogueEnded;

            case ConditionType.HasMoved:
                if (condition.Move == null)
                    return false;
                return condition.Move.HasMoved(condition.HasMoved, condition.HowManyTimes);

            case ConditionType.HasToCheckEmptynessInventory:
                return InventoryController.Instance.IsInventoryHasPlace();
            default:
                return false;

            case ConditionType.CanBePlacedInBalance:
                if (condition.balance == null)
                    return false;
                return condition.balance.CanAcceptItem(context.item);

            case ConditionType.WeightIsMoreThan:
                if (condition.balance == null)
                    return false;
                return condition.balance._currentWeight >= condition.weight;

            case ConditionType.WeightIsLessThan:
                if (condition.balance == null)
                    return false;
                return condition.balance._currentWeight <= condition.weight;

            case ConditionType.Exist:
                return condition.target != null;
        }
    }

    #endregion

    #region execute Action

    private void ExecuteAction(ActionEntry action, InteractionContext context)
    {


        switch (action.type)
        {
            case ActionType.SetActive:
                if (action.target != null)
                    action.target.SetActive(action.activeState);
                break;

            case ActionType.Open:
                if (action.target != null)
                    action.openDoor.Toggle(action.target);
                break;

            case ActionType.StartDialogue:
                if (action.npcDialogue != null && action.npcTalker != null)
                    action.npcTalker.StartDialogue(action.npcDialogue);
                break;

            case ActionType.PlaceObject:
                if (action.slot != null && action.itemPrefab != null)
                    action.slot.Fill(action.itemPrefab);
                break;

            case ActionType.ClearObject:
                if (action.slot != null && action.target != null)
                    action.slot.Clear();
                break;

            case ActionType.Move:
                if (action.Move != null)
                    action.Move.MoveInteraction(action.OffsetX, action.OffsetY);
                break;

            case ActionType.CycleSprites:
                if (action.cycleThroughSprite != null || action.sprites == null)
                    action.cycleThroughSprite.Cycle(action.sprites, action.cycle);
                break;

            case ActionType.Pick:
                if (action.pickable != null)
                    action.pickable.Pick(this.gameObject);
                break;

            case ActionType.ResetHasMoved:
                if (action.Move != null)
                    action.Move.ResetHasMoved();
                break;

            case ActionType.CallFunction:
                if (action.onExecute != null)
                    action.onExecute?.Invoke();
                break;

            case ActionType.Destroy:
                if (action.target != null)
                    Destroy(action.target);
                break;

            case ActionType.FillWithSprite:
                if (action.slot != null)
                    action.slot.FillWithSprite(action.item);
                break;

            case ActionType.PlaceInBalance:
                if (action.balance != null)
                    action.balance.TryAddItem(context.item);
                break;

            case ActionType.FeedBack:
                if (_interactionFeedBack != null)
                    _interactionFeedBack.TryFeedback();
                break;

            case ActionType.CloseDialogue:
                if (action.npcDialogue != null && action.npcTalker != null)
                    action.npcTalker.CloseBubble();
                break;

            case ActionType.Drop:
                if (action.slot != null && action.item != null)
                    WorldDropHandler.Instance.DropObject(new Vector3(action.slot.transform.localPosition.x, action.slot.transform.localPosition.y, 0),action.item);
                break;

            case ActionType.RemoveDraggedItem:
                if (action.slot != null && action.item != null)
                    action.inventoryController.RemoveInventoryItem(DragContext.DraggedItem);
                break;
        }
    }
    #endregion

    #region Call Try

    public void CallTry()
    {
        InteractionContext context = new InteractionContext
        {
        };

        TryExecuteAll(context);
    }

    #endregion

}