using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ActionEntry))]
public class ActionEntryDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var typeProp = property.FindPropertyRelative("type");
        ActionType type = (ActionType)typeProp.enumValueIndex;

        int lines = 2;

        switch (type)
        {
            case ActionType.SetActive:
                lines += 2;
                break;

            case ActionType.Open:
                lines += 2;
                break;
        }

        return lines * (EditorGUIUtility.singleLineHeight + 2f);
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
                EditorGUI.PropertyField(r, property.FindPropertyRelative("otherObject"));
                break;

            case ActionType.Open:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("otherObject"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("openDoor"));
                break;

        }

        EditorGUI.EndProperty();
    }
}