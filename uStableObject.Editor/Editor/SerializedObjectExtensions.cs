using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace                           uStableObject.EditorX
{
    public static class             SOExtensions
    {
        public static void          ListIterator(this SerializedObject so, SerializedProperty listProperty, Action<SerializedProperty, int>[] callbacks)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label(listProperty.displayName + " [" + listProperty.arraySize + "]");
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
                        so.ApplyModifiedProperties();
                    }
                    if (listProperty.arraySize > 0)
                    {
                        if (GUILayout.Button("-", GUILayout.Width(30)))
                        {
                            int initialSize = listProperty.arraySize;
                            int removingElementIndex = listProperty.arraySize - 1;
                            var arrayElement = listProperty.GetArrayElementAtIndex(removingElementIndex);
                            Debug.Log("Removing " + arrayElement.displayName + " from " + listProperty.displayName);
                            listProperty.DeleteArrayElementAtIndex(removingElementIndex);
                            if (listProperty.arraySize == initialSize) //for Objects have to call this twice
                            {
                                listProperty.DeleteArrayElementAtIndex(removingElementIndex);
                            }
                            so.ApplyModifiedProperties();
                        }
                    }
                    else
                    {
                        GUILayout.Space(30);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    //GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    {
                        for (int i = 0; i < listProperty.arraySize; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                foreach (var callback in callbacks)
                                {
                                    callback(listProperty, i);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }
        
        public static void          ShowSeparatorAND(SerializedProperty listProperty, int index)
        {
            if (index == 0)
            {
                GUILayout.Space(38);
            }
            else
            {
                GUILayout.BeginVertical(GUILayout.Width(30));
                {
                    GUILayout.Label("AND", GUILayout.Width(30));
                    if (GUILayout.Button("^", GUILayout.Width(30)))
                    {
                        listProperty.MoveArrayElement(index, index - 1);
                    }
                }
                GUILayout.EndVertical();
            }
        }
        
        public static void          ShowSeparatorOR(SerializedProperty listProperty, int index)
        {
            if (index == 0)
            {
                GUILayout.Space(34);
            }
            else
            {
                GUILayout.Label("OR", GUILayout.Width(26));
            }
        }

        public static void          ShowEmbededInspector(this SerializedProperty childProperty, string featureName, string title, ref Editor cachedEditor, Color color)
        {
            Color prevColor = GUI.backgroundColor;
            EditorGUILayout.BeginVertical(GUI.skin.textArea);
            {
                GUI.backgroundColor = EditorGUIUtility.isProSkin ? color : color * 0.6f;
                EditorGUILayout.BeginHorizontal(GUI.skin.textArea);
                {
                    GUI.backgroundColor = prevColor;
                    if (childProperty.propertyType == SerializedPropertyType.ObjectReference
                        && childProperty.objectReferenceValue)
                    {
                        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
                        EditorGUILayout.PropertyField(childProperty, GUIContent.none, GUILayout.MaxWidth(65));
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(childProperty, GUIContent.none);
                    }
                }
                EditorGUILayout.EndHorizontal();
                if (childProperty.propertyType == SerializedPropertyType.ObjectReference
                    && childProperty.objectReferenceValue)
                {
                    //EditorGUI.indentLevel++;
                    Editor.CreateCachedEditor(childProperty.objectReferenceValue, null, ref cachedEditor);
                    cachedEditor.OnInspectorGUI();
                    //EditorGUI.indentLevel--;
                    GUILayout.Space(16);
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
