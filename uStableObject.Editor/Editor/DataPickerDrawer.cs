using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using uStableObject.Utilities;

namespace                                   uStableObject
{
    [CustomPropertyDrawer(typeof(DataPicker), true)]
    public class                            DataPickerDrawer : PropertyDrawer
    {
        Editor                              _editor;

        static bool                         IsExternal;
        static Dictionary<object, bool>     FoldedEntries = new Dictionary<object, bool>();
        static Dictionary<string, Type[]>   AcceptedTypes = new Dictionary<string, Type[]>();
        static Dictionary<string, string[]> AcceptedTypesStrings = new Dictionary<string, string[]>();

        public override float               GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }

        public override void                OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DataPicker dataPicker = this.attribute as DataPicker;
            var typeBase = dataPicker.BaseType ?? this.fieldInfo.FieldType;
            var color = dataPicker.Color;
            position.x += 5;
            position.width -= 5;
            EditorGUI.BeginProperty(position, label, property);
            this.ShowChildProperty(property, "", label, ref this._editor, typeBase, dataPicker.AllowBaseType, color);
            EditorGUI.EndProperty();
            if (property == null && property.serializedObject != null)
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        void                                ShowChildProperty(SerializedProperty childProperty, string featureName, GUIContent title, ref Editor cachedEditor, Type acceptedType, bool allowBaseType, Color color)
        {
            bool                            toggledOn = true;

            GUILayout.ExpandWidth(false);
            Color prevColor = GUI.backgroundColor;
            GUI.backgroundColor = EditorGUIUtility.isProSkin ? color : color * 0.6f;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(30);
            EditorGUILayout.BeginHorizontal(GUI.skin.textArea);// EditorStyles.helpBox);//
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            if (childProperty.objectReferenceValue)
            {
                //Check forexternal asset
                string propertyAssetPath = AssetDatabase.GetAssetPath(childProperty.objectReferenceValue);
                string seriliazedObjectPath = AssetDatabase.GetAssetPath(childProperty.serializedObject.targetObject);
                if (IsExternal != (string.Compare(propertyAssetPath, seriliazedObjectPath) != 0))
                {
                    IsExternal = !IsExternal;
                    Debug.Log("this._isExternal=" + IsExternal);
                    AcceptedTypes.Remove(acceptedType.Name);
                    AcceptedTypesStrings.Remove(acceptedType.Name);
                }

                if (!FoldedEntries.TryGetValue(childProperty.objectReferenceValue, out toggledOn))
                {
                    FoldedEntries.Add(childProperty.objectReferenceValue, toggledOn);
                }
                if (EditorGUILayout.ToggleLeft(title, toggledOn, EditorStyles.boldLabel, GUILayout.MinWidth(100)) != toggledOn)
                {
                    toggledOn = !toggledOn;
                    FoldedEntries[childProperty.objectReferenceValue] = toggledOn;
                }
                //string typeName = childProperty.objectReferenceValue.GetType().Name;
                //EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            else
            {
                EditorGUILayout.Space(14);
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUI.indentLevel = indentLevel;
            if (this.ShowAvailableTypes(childProperty, acceptedType, allowBaseType, featureName, title.text))
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();
                GUI.backgroundColor = prevColor;
            }
            else 
            {
                EditorGUILayout.PropertyField(childProperty, GUIContent.none, true, GUILayout.MinWidth(100), GUILayout.MaxWidth(36));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();
                GUI.backgroundColor = prevColor;
                if (childProperty.objectReferenceValue && toggledOn)
                {
                    EditorGUI.indentLevel++;
                    Editor.CreateCachedEditor(childProperty.objectReferenceValue, null, ref cachedEditor);
                    cachedEditor.OnInspectorGUI();
                    EditorGUI.indentLevel--;
                    GUILayout.Space(16);
                }
            }
        }

        bool                                ShowAvailableTypes(SerializedProperty childProperty, Type baseType, bool allowBaseType, string featureName, string title)
        {
            Type[]                          types;
            string[]                        typesStrings;
            int                             currentTypeIndex;

            if (!AcceptedTypes.TryGetValue(baseType.Name, out types)
                || !AcceptedTypesStrings.TryGetValue(baseType.Name, out typesStrings))
            {
                if (allowBaseType)
                {
                    types = Assembly
                            .GetAssembly(baseType)
                            .GetTypes()
                            .Where(t => t.BaseType == baseType || t == baseType).ToArray();
                }
                else
                {
                    types = Assembly
                            .GetAssembly(baseType)
                            .GetTypes()
                            .Where(t => t.BaseType == baseType).ToArray();
                }
                typesStrings = new string[types.Length + 1];
                typesStrings[0] = "-";
                for (var i = 1; i < typesStrings.Length; ++i)
                {
                    typesStrings[i] = types[i - 1].Name.Replace(baseType.Name, "");
                    if (string.IsNullOrEmpty(typesStrings[i]))
                    {
                        typesStrings[i] = "(Simple)";
                    }
                    if (IsExternal)
                    {
                        typesStrings[i] = "[External] " +  typesStrings[i];
                    }
                }
                if (!AcceptedTypes.ContainsKey(baseType.Name))
                    AcceptedTypes.Add(baseType.Name, types);
                else
                    AcceptedTypes[baseType.Name] = types;
                if (!AcceptedTypesStrings.ContainsKey(baseType.Name))
                    AcceptedTypesStrings.Add(baseType.Name, typesStrings);
                else
                    AcceptedTypesStrings.Add(baseType.Name, typesStrings);
            }
            currentTypeIndex = childProperty.objectReferenceValue ? Array.IndexOf(types, childProperty.objectReferenceValue.GetType()) + 1 : 0;
            int newTypeIndex = EditorGUILayout.Popup(currentTypeIndex, typesStrings, GUILayout.MinWidth(150));
            if (newTypeIndex != currentTypeIndex)
            {
                if (newTypeIndex > 0) // set value
                {
                    --newTypeIndex;
                    if (IsExternal)
                    {
                        childProperty.objectReferenceValue = null;
                        Debug.Log("Broke reference to external asset, now creating embeded asset instead.");
                    }
                    if (childProperty.objectReferenceValue == null)
                    {
                        Undo.RecordObject(childProperty.serializedObject.targetObject, "CREATE_ASSET");
                        var name = childProperty.serializedObject.targetObject.name.Substring(childProperty.serializedObject.targetObject.name.LastIndexOf(" - ") + 3);
                        var headingName = (!string.IsNullOrEmpty(featureName) ? featureName + " - " : "") + title + " - ";
                        childProperty.objectReferenceValue = ScriptableUtils.AddAsChild(types[newTypeIndex], headingName + name, childProperty.serializedObject.targetObject, false);

                        FoldedEntries.Add(childProperty.objectReferenceValue, true);
                    }
                    else
                    {
                        FoldedEntries.Remove(childProperty.objectReferenceValue);
                        /*
                        var name = childProperty.objectReferenceValue.name;
                        AssetDatabase.RemoveObjectFromAsset(childProperty.objectReferenceValue);
                        childProperty.objectReferenceValue = ScriptableUtils.AddAsChild(types[newTypeIndex], name, this.serializedObject.targetObject, false);
                        */

                        Undo.RecordObject(childProperty.serializedObject.targetObject, "MODIFY_ASSET");
                        var newInstance = ScriptableObject.CreateInstance(types[newTypeIndex]);
                        MonoScript monoScript = MonoScript.FromScriptableObject(newInstance);
                        SerializedObject so = new SerializedObject(childProperty.objectReferenceValue);
                        SerializedProperty scriptProperty = so.FindProperty("m_Script");
                        so.Update();
                        scriptProperty.objectReferenceValue = monoScript;
                        so.ApplyModifiedProperties();

                        FoldedEntries.Add(so.targetObject, true);
                    }
                }
                else // unset value
                {
                    if (IsExternal)
                    {
                        childProperty.objectReferenceValue = null;
                        Debug.LogError("Deletion blocked for external assets to prevent user error with side effects. Delete directly or use the ScriptableTools windows if needed.");
                    }
                    else
                    {
                        FoldedEntries.Remove(childProperty.objectReferenceValue);
                        Undo.RecordObject(childProperty.serializedObject.targetObject, "DELETE_ASSET");
                        AssetDatabase.RemoveObjectFromAsset(childProperty.objectReferenceValue);
                        Undo.RecordObject(childProperty.objectReferenceValue, "DELETE_ASSET");
                        UnityEngine.Object.DestroyImmediate(childProperty.objectReferenceValue);
                        childProperty.objectReferenceValue = null;
                        throw new Exception("DELETED ASSET SUCCESSFULLY");
                    }
                }
                return (true);
            }
            return (false);
        }
    }
}
