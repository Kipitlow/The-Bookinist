using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.ObjectSpin;

public class InteractionRunner : MonoBehaviour
{
    [SerializeField] private List<ConditionEntry> conditions;
    [SerializeField] private List<ActionEntry> actions;

    public void TryExecute(InteractionContext context)
    {
        foreach (var condition in conditions)
        {
            if (!EvaluateCondition(condition, context))
                return;
        }

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
                if (condition.thisObject == null)
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