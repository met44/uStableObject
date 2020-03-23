using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace                               uStableObject.Utilities
{
    public static class                 ScriptableUtils
    {
        public static bool              AddAsChildValidation()
        {
#if UNITY_EDITOR
            return (UnityEditor.Selection.activeObject is ScriptableObject);
#else
            return (false);
#endif
        }

        //[ContextMenu("Add Event Listener Child")]
        public static T                 AddAsChild<T>(string name, Object parent = null, bool autoSelect = true) where T : ScriptableObject
        {
            T                           child = default(T);
#if UNITY_EDITOR

            if (UnityEditor.Selection.activeObject is ScriptableObject)
            {
                var data = parent ?? UnityEditor.Selection.activeObject;
                child = ScriptableObject.CreateInstance<T>();
                child.name = name;
                UnityEditor.AssetDatabase.AddObjectToAsset(child, data);
                UnityEditor.EditorUtility.SetDirty(data);
                UnityEditor.AssetDatabase.SaveAssets();
                if (autoSelect)
                {
                    UnityEditor.Selection.activeObject = child;
                }
                else
                {
                    UnityEditor.EditorGUIUtility.PingObject(child);
                }
            }
#endif
            return (child);
        }

        public static Object            AddAsChild(System.Type type, string name, Object parent = null, bool autoSelect = true)
        {
            Object                      child = null;
#if UNITY_EDITOR

            if (UnityEditor.Selection.activeObject is ScriptableObject)
            {
                var data = parent ?? UnityEditor.Selection.activeObject;
                child = ScriptableObject.CreateInstance(type);
                child.name = name;
                UnityEditor.AssetDatabase.AddObjectToAsset(child, data);
                UnityEditor.EditorUtility.SetDirty(data);
                UnityEditor.AssetDatabase.SaveAssets();
                if (autoSelect)
                {
                    UnityEditor.Selection.activeObject = child;
                }
                else
                {
                    UnityEditor.EditorGUIUtility.PingObject(child);
                }
            }
#endif
            return (child);
        }

        //[ContextMenu("Add Event Listener Child")]
        public static T                 AddInSameFolder<T>(string name, Object parent = null, bool autoSelect = true) where T : ScriptableObject
        {
            T                           newInstance = default(T);
#if UNITY_EDITOR

            if (UnityEditor.Selection.activeObject is ScriptableObject)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(parent);
                path = path.Substring(0, path.LastIndexOf("/") + 1);
                path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
                newInstance = ScriptableObject.CreateInstance<T>();
                newInstance.name = name;
                UnityEditor.AssetDatabase.CreateAsset(newInstance, path);
                UnityEditor.AssetDatabase.SaveAssets();
                if (autoSelect)
                {
                    UnityEditor.Selection.activeObject = newInstance;
                }
                else
                {
                    UnityEditor.EditorGUIUtility.PingObject(newInstance);
                }
            }
#endif
            return (newInstance);
        }

        public static void              SwapScript(Object instanceRef, System.Type type, string name = "")
        {
#if UNITY_EDITOR
            var newInstance = ScriptableObject.CreateInstance(type);
            MonoScript monoScript = MonoScript.FromScriptableObject(newInstance);
            SerializedObject so = new SerializedObject(instanceRef);
            SerializedProperty scriptProperty = so.FindProperty("m_Script");
            so.Update();
            scriptProperty.objectReferenceValue = monoScript;
            so.ApplyModifiedProperties();
            if (!string.IsNullOrEmpty(name))
            {
                so.targetObject.name = name;
            }
            EditorUtility.SetDirty(so.targetObject);
#endif
        }

#if UNITY_EDITOR
        [MenuItem("Assets/[Set as main asset]")]
        static void                     SetAsMainAsset()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            AssetDatabase.SetMainObject(Selection.activeObject, path);
            AssetDatabase.ImportAsset(path);
        }

        [MenuItem("Assets/[Set as main asset]", true)]
        static bool                     SetAsMainAssetValidation()
        {
            return (Selection.objects != null
                    && Selection.objects.Length == 1
                    && Selection.activeObject is ScriptableObject 
                    && !AssetDatabase.IsMainAsset(Selection.activeObject));
        }
#endif
    }
}
