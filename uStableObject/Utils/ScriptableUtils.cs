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
        public static void              AddAsChild(System.Type type, string name)
        {
#if UNITY_EDITOR
            ScriptableObject            child;

            if (UnityEditor.Selection.activeObject is ScriptableObject)
            {
                var data = UnityEditor.Selection.activeObject;
                child = ScriptableObject.CreateInstance(type);
                child.name = name + type.ToString();
                UnityEditor.AssetDatabase.AddObjectToAsset(child, data);
                UnityEditor.EditorUtility.SetDirty(data);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.Selection.activeObject = child;
            }
#endif
        }
    }
}
