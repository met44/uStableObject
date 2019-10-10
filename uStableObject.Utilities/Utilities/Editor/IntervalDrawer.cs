using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    [CustomPropertyDrawer(typeof(Interval))]
    [CustomPropertyDrawer(typeof(IntervalF))]
    public class                IntervalDrawer : PropertyDrawer
    {
        public override float   GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 16;
        }

        public override void    OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var fromProperty = property.FindPropertyRelative("_from");
            var toProperty = property.FindPropertyRelative("_to");
            position.width *= .41f;
            EditorGUI.LabelField(position, label);
            position.width /= 2;
            position.x = position.width * 2;
            EditorGUI.PropertyField(position, fromProperty, GUIContent.none);
            position.x += position.width;
            EditorGUI.PropertyField(position, toProperty, GUIContent.none);
            EditorGUI.EndProperty();
        }
    }
}
