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
        public static T                 AddAsChild<T>(string name) where T : ScriptableObject
        {
            T                           child = default(T);
#if UNITY_EDITOR

            if (UnityEditor.Selection.activeObject is ScriptableObject)
            {
                var data = UnityEditor.Selection.activeObject;
                child = ScriptableObject.CreateInstance<T>();
                child.name = name;
                UnityEditor.AssetDatabase.AddObjectToAsset(child, data);
                UnityEditor.EditorUtility.SetDirty(data);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.Selection.activeObject = child;
            }
#endif
            return (child);
        }
    }
}
