using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InteractionSet
{
    #region Variables

    [Tooltip("help to understand the effect of this interaction but not mandatory")]
    public string interactionName;
    public List<ConditionEntry> conditions = new();
    public List<ActionEntry> actions = new();

    #endregion
}