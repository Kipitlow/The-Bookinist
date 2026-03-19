using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRunner : MonoBehaviour
{
    [SerializeField] private List<InteractionSet> interactionSets = new();

    public void TryExecuteAll(InteractionContext context)
    {
        foreach (var set in interactionSets)
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

                return condition.thisObject.layer == condition.otherObject.layer;

            case ConditionType.SameZone:
                if (condition.zone == null || condition.otherObject == null)
                    return false;

                return condition.zone.Contains(condition.otherObject);
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
                if (action.otherObject != null)
                    action.otherObject.SetActive(action.activeState);
                break;

            case ActionType.Open:
                if (action.otherObject != null)
                    action.openDoor.Toggle(action.otherObject);
                break;
        }
    }
}