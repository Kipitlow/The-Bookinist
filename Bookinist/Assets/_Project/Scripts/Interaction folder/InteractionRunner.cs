using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRunner : MonoBehaviour
{
    [SerializeField] private List<InteractionSet> _interactionSets = new();


    public void TryExecuteAll(InteractionContext context)
    {
        foreach (var set in _interactionSets)
        {
            if (AreConditionsValid(set.conditions, context))
            {
                ExecuteActions(set.actions, context);
            }
        }
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

    private bool EvaluateCondition(ConditionEntry condition, InteractionContext context)
    {
        switch (condition.type)
        {
            case ConditionType.SameLayer:
                if (condition.thisObject == null || condition.otherObject == null)
                    return false;

                return condition.thisObject.layer == condition.target.layer;

            case ConditionType.SameZone:
                if (condition.zone == null || condition.target == null)
                    return false;

                return condition.zone.IsInside(condition.target);

            case ConditionType.OnTouch:
                if (context.target == null)
                    return false;
                return this.gameObject == context.target;
;

            default:
                return false;
        }
    }

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

            case ActionType.Print:
                if (action.printText != null && action.text != null )
                    action.printText.Print(action.text);
                break;
        }
    }
}