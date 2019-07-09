using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/MatchCount", order = 2)]
    public class                            MatchAmountVar : BoolVar
    {
        [SerializeField] IntVar[]           _vars;
        [SerializeField] int                _targetAmount;
        [SerializeField] Operators          _operator = Operators.Equal;

        public override bool                Value
        {
            get
            {
                int amount = 0;
                foreach (var intVar in this._vars)
                {
                    amount += intVar;
                }
                switch (this._operator)
                {
                    case Operators.Superior: return (amount > this._targetAmount);
                    case Operators.Inferior: return (amount < this._targetAmount);
                }
                return (amount == this._targetAmount);
            }
            set
            {
                Debug.LogError("This is a readonly var: " + this.name);
            }
        }

        public enum                         Operators
        {
            Equal, Superior, Inferior
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/uStableObject/Var/Tests/As Child - MatchAmount")]
        public static void                      AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild<MatchAmountVar>("MatchCount - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/Var/Tests/As Child - MatchAmount", true)]
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
            switch (this._operator)
            {
                case Operators.Superior:        this.name += " Superior To";   break;
                case Operators.Inferior:        this.name += " Inferior To";   break;
                case Operators.Equal:           this.name += " Equal To";   break;
            }
            this.name += " " + this._targetAmount; 
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}

