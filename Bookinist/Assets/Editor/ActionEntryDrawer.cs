using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ActionEntry))]
public class ActionEntryDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var typeProp = property.FindPropertyRelative("type");
        ActionType type = (ActionType)typeProp.enumValueIndex;

        float lines = 2;
        float lineHeight = EditorGUIUtility.singleLineHeight + 2f;
        float extraHeight = 0f;

        switch (type)
        {
            case ActionType.SetActive:
                lines += 2;
                break;

            case ActionType.Open:
                lines += 2;
                break;

            case ActionType.StartDialogue:
                lines += 2;
                break;

            case ActionType.CallFunction:
                var onExecuteProp = property.FindPropertyRelative("onExecute");
                var callsProp = onExecuteProp.FindPropertyRelative("m_PersistentCalls.m_Calls");
                int callCount = 0;
                if (callsProp != null) callCount = callsProp.arraySize;

                if (callCount > 0) lines += 3.7f;
                else lines += 5;

                break;

            case ActionType.PlaceObject:
                lines += 2;
                break;

            case ActionType.ClearObject:
                lines += 1;
                break;

            case ActionType.Move:
                lines += 3;
                break;

            case ActionType.ResetHasMoved:
                lines += 1;
                break;

            case ActionType.Pick:
                lines += 1;
                break;

            case ActionType.Drop:
                lines += 2;
                break;

            case ActionType.Destroy:
                lines += 0;
                break;

            case ActionType.CycleSprites:
                lines += 2;

                var spritesProp = property.FindPropertyRelative("sprites");
                extraHeight += EditorGUI.GetPropertyHeight(spritesProp, true);
                break;

            case ActionType.PlaceInBalance:
                lines += 1;
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

        ActionType type = (ActionType)typeProp.enumValueIndex;

        switch (type)
        {
            case ActionType.SetActive:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("activeState"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("target"));
                break;

            case ActionType.Open:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("target"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("openDoor"));
                break;

            case ActionType.StartDialogue:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("npcDialogue"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("npcTalker"));
                break;

            case ActionType.CallFunction:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("onExecute"));
                r.y += h + s;
                break;

            case ActionType.PlaceObject:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("slot"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("itemPrefab"));
                break;

            case ActionType.ClearObject:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("slot"));
                r.y += h + s;
                break;

            case ActionType.Move:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("Move"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("OffsetX"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("OffsetY"));
                break;

            case ActionType.ResetHasMoved:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("Move"));
                break;

            case ActionType.Pick:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("pickable"));
                break;

            case ActionType.Drop:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("slot"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("item"));
                break;

            case ActionType.Destroy:
                break;

            case ActionType.CycleSprites:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("cycleThroughSprite"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("cycle"));
                r.y += h + s;
                var spritesProp = property.FindPropertyRelative("sprites");
                float spritesHeight = EditorGUI.GetPropertyHeight(spritesProp, true);
                Rect spritesRect = new Rect(r.x, r.y, r.width, spritesHeight);
                EditorGUI.PropertyField(spritesRect, spritesProp, true);
                r.y += spritesHeight + s;

                break;

            case ActionType.PlaceInBalance:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("balance"));
                break;
        }
        EditorGUI.EndProperty();
    }
}