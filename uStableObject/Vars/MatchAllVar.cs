using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/MatchAll", order = 2)]
    public class                            MatchAllVar : BoolVar
    {
        [SerializeField] BoolVar[]          _vars;

        public override bool                Value
        {
            get
            {
                foreach (var boolVar in this._vars)
                {
                    if (!boolVar)
                    {
                        return (false);
                    }
                }
                return (true);
            }
            set
            {
                Debug.LogError("This is a readonly var: " + this.name);
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/uStableObject/Var/Tests/As Child - MatchAll")]
        public static void                      AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild<MatchAllVar>("MatchCount - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/Var/Tests/As Child - MatchAll", true)]
        public static bool                      AddTypeAsChildValidation()
        {
            return (UnityEditor.Selection.activeObject is ScriptableObject);
        }

        [ContextMenu("DELETE")]
        public void                             RemoveAsset()
        {
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(UnityEditor.Selection.activeObject);
            UnityEditor.EditorUtility.SetDirty(UnityEditor.Selection.activeObject);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("DELETE", true)]
        public bool                             RemoveAssetValidation()
        {
            return (!UnityEditor.AssetDatabase.IsMainAsset(UnityEditor.Selection.activeObject));
        }

        [ContextMenu("Init name")]
        public void                             MatchEventName()
        {
            this.name = "";
            foreach (var val in this._vars)
            {
                int spaceIndex = val.name.LastIndexOf(' ');
                this.name += val.name.Substring(spaceIndex + 1);
            }
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}

