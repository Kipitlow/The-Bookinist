using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionEntry))]
public class ConditionEntryDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var typeProp = property.FindPropertyRelative("type");
        ConditionType type = (ConditionType)typeProp.enumValueIndex;

        float lines = 2;
        float lineHeight = EditorGUIUtility.singleLineHeight + 2f;
        float extraHeight = 0f;

        switch (type)
        {
            case ConditionType.SameLayer:
                lines += 2;
                break;

            case ConditionType.SameZone:
                lines += 2;
                break;

            case ConditionType.OnTouch:
                lines += 0;
                break;

            case ConditionType.IsEmpty:
                lines += 2;
                break;

            case ConditionType.IsSameItemSO:
                lines += 2;
                break;

            case ConditionType.IsNotSameItemSO:
                lines += 2;
                break;

            case ConditionType.HasDialogueStarted:
                lines += 1;
                break;

            case ConditionType.HasDialogueEnded:
                lines += 1;
                break;

            case ConditionType.HasMoved:
                lines += 3;
                break;

            case ConditionType.OnWichFrame:
                lines += 3;
                break;

            case ConditionType.CanBePlacedInBalance:
                lines += 1;
                break;

            case ConditionType.ISBookFinish:
                lines += 1;
                break;

            case ConditionType.WeightIsMoreThan:
                lines += 2;
                break;

            case ConditionType.WeightIsLessThan:
                lines += 2;
                break;
        }

        return lines * lineHeight + extraHeight;

    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float h = EditorGUIUtility.singleLineHeight;
        float s = 2f;
        Rect r = new Rect(position.x, position.y, position.width, h);

        EditorGUI.LabelField(r, label, EditorStyles.boldLabel);
        r.y += h + s;

        var typeProp = property.FindPropertyRelative("type");
        EditorGUI.PropertyField(r, typeProp);
        r.y += h + s;

        ConditionType type = (ConditionType)typeProp.enumValueIndex;

        switch (type)
        {
            case ConditionType.SameLayer:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("layerDetector"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("target"));
                break;

            case ConditionType.SameZone:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("zone"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("target"));
                break;

            case ConditionType.OnTouch:
                break;

            case ConditionType.IsEmpty:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("slot"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("shouldBeEmpty"));
                break;

            case ConditionType.IsSameItemSO:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("selectedItemIsWanted"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("item"));
                break;

            case ConditionType.IsNotSameItemSO:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("selectedItemIsWanted"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("item"));
                break;

            case ConditionType.HasDialogueStarted:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("npcTalker"));
                r.y += h + s;
                break;

            case ConditionType.HasDialogueEnded:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("npcTalker"));
                r.y += h + s;
                break;

            case ConditionType.HasMoved:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("Move"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("HasMoved"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("HowManyTimes"));
                break;

            case ConditionType.OnWichFrame:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("cycleThroughSprite"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("WantedFrame"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("trueIfMore"));
                break;

            case ConditionType.CanBePlacedInBalance:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("balance"));
                break;

            case ConditionType.ISBookFinish:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("isBookFinish"));
                break;

            case ConditionType.WeightIsMoreThan:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("weight"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("balance"));
                break;

            case ConditionType.WeightIsLessThan:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("weight"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("balance"));
                break;
        }

        EditorGUI.EndProperty();
    }
}