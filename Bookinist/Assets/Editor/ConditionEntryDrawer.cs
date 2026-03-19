using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionEntry))]
public class ConditionEntryDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var typeProp = property.FindPropertyRelative("type");
        ConditionType type = (ConditionType)typeProp.enumValueIndex;

        int lines = 2; // titre + type

        switch (type)
        {
            case ConditionType.SameLayer:
                lines += 2;
                break;

            case ConditionType.SameZone:
                lines += 3;
                break;

            case ConditionType.OnTouch:
                lines += 1;
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

        ConditionType type = (ConditionType)typeProp.enumValueIndex;

        switch (type)
        {
            case ConditionType.SameLayer:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("thisObject"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("otherObject"));
                break;

            case ConditionType.SameZone:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("zone"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("thisObject"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("otherObject"));
                break;

            case ConditionType.OnTouch:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("thisObject"));
                r.y += h + s;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("otherObject"));
                r.y += h + s;
                break;
        }

        EditorGUI.EndProperty();
    }
}