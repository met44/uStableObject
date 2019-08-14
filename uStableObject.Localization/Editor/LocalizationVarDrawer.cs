using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace                       uStableObject.Data.Localization
{
    [CustomPropertyDrawer(typeof(LocalizationVar))]
    public class                LocalizationVarDrawer : PropertyDrawer
    {
        public override void    OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (property.objectReferenceValue)
            {
                // Calculate rects
                var textRect = new Rect(position.x, position.y, position.width - 40, position.height);
                var assetRect = new Rect(textRect.xMax + 2, position.y, 35, position.height);
                LocalizationVar targetInstance = property.objectReferenceValue as LocalizationVar;
                string text = EditorGUI.TextField(textRect, targetInstance.Original);
                if (string.Compare(text, targetInstance.Original) != 0)
                {
                    targetInstance.Original = text;
                    EditorUtility.SetDirty(targetInstance);
                }
                property.objectReferenceValue = EditorGUI.ObjectField(assetRect, GUIContent.none, property.objectReferenceValue, typeof(LocalizationVar), false);
            }
            else
            {
                var assetRect = new Rect(position.x, position.y, position.width - 35, position.height);
                var addButtonRect = new Rect(assetRect.xMax + 5, position.y, 30, position.height);
                property.objectReferenceValue = EditorGUI.ObjectField(assetRect, GUIContent.none, property.objectReferenceValue, typeof(LocalizationVar), false);
                if (GUI.Button(addButtonRect, "+"))
                {
                    var hostObjectTypeName = property.serializedObject.targetObject.GetType().Name;
                    string locName = "Localization - " + hostObjectTypeName + " - " + property.displayName;
                    string locHint = hostObjectTypeName + " " + property.displayName;
                    string locOriginal = property.displayName;
                    property.objectReferenceValue = LocalizationManager.GetOrCreateLocAsset(locName, locHint, locOriginal, true);
                }
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
