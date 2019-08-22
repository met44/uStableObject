using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    }
}
