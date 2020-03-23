using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace                       uStableObject.Data.Localization
{
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(LocalizationVar))]
    public class                LocalizationVarDrawer : PropertyDrawer
    {
        float                   _width = 100;
        float                   _height = EditorGUIUtility.singleLineHeight;

        public override float   GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue)
            {
                return (this._height);
            }
            else
            {
                return base.GetPropertyHeight(property, label);
            }
        }

        public override void    OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.serializedObject.isEditingMultipleObjects)
            {
                EditorGUI.PropertyField(position, property);
                EditorGUI.EndProperty();
                return;
            }

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (Event.current.type == EventType.Repaint)
            {
                this._width = Mathf.Abs(position.width);
                if (property.objectReferenceValue)
                {
                    LocalizationVar targetInstance = property.objectReferenceValue as LocalizationVar;
                    this._height = ((int)(GUI.skin.textArea.CalcHeight(new GUIContent(targetInstance.Original), this._width - 40) / EditorGUIUtility.singleLineHeight) + 1) * EditorGUIUtility.singleLineHeight;
                }
            }

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (property.objectReferenceValue)
            {
                var prettyPrint = this.GetPrettyPrintPropertyPath(property);
                // Calculate rects
                var textRect = new Rect(position.x, position.y, position.width - 40, position.height);
                var assetRect = new Rect(textRect.xMax + 2, position.y, 35, EditorGUIUtility.singleLineHeight);
                LocalizationVar targetInstance = property.objectReferenceValue as LocalizationVar;
                if (targetInstance)
                {
                    GUI.skin.textArea.wordWrap = true;
                    string text = EditorGUI.TextArea(textRect, targetInstance.Original, GUI.skin.textArea);
                    if (string.Compare(text, targetInstance.Original) != 0)
                    {
                        targetInstance.Original = text;
                        EditorUtility.SetDirty(targetInstance);
                    }
                }
                property.objectReferenceValue = EditorGUI.ObjectField(assetRect, GUIContent.none, property.objectReferenceValue, typeof(LocalizationVar), false);
            }
            else
            {
                var assetRect = new Rect(position.x, position.y, position.width - 65, position.height);
                var addButtonRect = new Rect(assetRect.xMax + 5, position.y, 30, position.height);
                var stackAddButtonRect = new Rect(addButtonRect.xMax, position.y, 30, position.height);
                property.objectReferenceValue = EditorGUI.ObjectField(assetRect, GUIContent.none, property.objectReferenceValue, typeof(LocalizationVar), false);
                if (GUI.Button(addButtonRect, "+"))
                {
                    foreach (var targetObject in property.serializedObject.targetObjects)
                    {
                        if (targetObject is LabelLocalization)
                        {
                            (targetObject as LabelLocalization).CreateLocAsset();
                        }
                        else
                        {
                            var hostObjectName = targetObject.name;
                            var prettyPrint = this.GetPrettyPrintPropertyPath(property);
                            string locName = "Localization - " + hostObjectName + " - " + prettyPrint;
                            string locHint = hostObjectName + " " + prettyPrint;
                            string locOriginal = property.displayName;
                            property.objectReferenceValue = LocalizationManager.GetOrCreateLocAsset(locName, locHint, locOriginal, true);
                        }
                    }
                }
                if (GUI.Button(stackAddButtonRect, "V"))
                {
                    foreach (var targetObject in property.serializedObject.targetObjects)
                    {
                        if (targetObject is LabelLocalization)
                        {
                            (targetObject as LabelLocalization).CreateLocAsset();
                        }
                        else
                        {
                            var hostObjectName = targetObject.name;
                            var prettyPrint = this.GetPrettyPrintPropertyPath(property);
                            string locName = "Localization - " + hostObjectName + " - " + prettyPrint;
                            string locHint = hostObjectName + " " + prettyPrint;
                            string locOriginal = property.displayName;
                            property.objectReferenceValue = LocalizationManager.GetOrCreateLocAsset(locName, locHint, locOriginal, true);
                            if (targetObject is ScriptableObject)
                            {
                                string sourcePath = AssetDatabase.GetAssetPath(property.objectReferenceValue);
                                AssetDatabase.RemoveObjectFromAsset(property.objectReferenceValue);
                                AssetDatabase.DeleteAsset(sourcePath);
                                AssetDatabase.AddObjectToAsset(property.objectReferenceValue, targetObject);
                                EditorUtility.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                                if (AssetDatabase.IsMainAsset(property.objectReferenceValue))
                                {
                                    Debug.LogError("Locvar became main asset, fixing...");
                                    string assetPath = AssetDatabase.GetAssetPath(targetObject);
                                    AssetDatabase.SetMainObject(targetObject, assetPath);
                                    AssetDatabase.SaveAssets();
                                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                                    if (AssetDatabase.IsMainAsset(property.objectReferenceValue))
                                    {
                                        Debug.LogError("ERROR: Locvar is still the main asset");
                                    }
                                    else
                                    {
                                        Debug.Log("SUCCESS: Locvar is no longer the main asset !");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        string                  GetPrettyPrintPropertyPath(SerializedProperty property)
        {
            int parentPropertyNameIndex = property.propertyPath.LastIndexOf(".");
            if (parentPropertyNameIndex != -1)
            {
                string parentPath = property.propertyPath.Substring(0, parentPropertyNameIndex);
                var parentProperty = property.serializedObject.FindProperty(parentPath);
                /*if (parentProperty.displayName != "Array" && !parentProperty.displayName.StartsWith("_data"))
                {
                    return (this.GetPrettyPrintPropertyPath(parentProperty) + parentProperty.displayName + property.displayName);
                }*/
                if (property.displayName != "Array" && !property.displayName.StartsWith("_data"))
                {
                    return (this.GetPrettyPrintPropertyPath(parentProperty) + " - " + property.displayName);
                }
                return (this.GetPrettyPrintPropertyPath(parentProperty));
            }
            return (property.displayName);
        }
    }
}
