using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                       uStableObject.Utilities
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Player Prefs", order = 0)]
    public class                                PlayerPrefsVars : ScriptableObject
    {
        #region Input Data
        [SerializeField] bool                   _clearAll;
        [SerializeField] ScriptableObject[]     _dataVars;
        #endregion

        #region Unity
        /* better to rely on manual trigger for this
        void                                    OnEnable()
        {
            this.Load();
        }
        */
        #endregion

        #region Trigger
        public void                             Load()
        {
            if (this._clearAll)
            {
                PlayerPrefs.DeleteAll();
            }
            foreach (var dataVar in this._dataVars)
            {
                if (dataVar is IBaseTypeVar<int>)
                {
                    var intVar = dataVar as IBaseTypeVar<int>;
                    if (!intVar.HasRuntimeValue())
                    {
                        intVar.Value = PlayerPrefs.GetInt(dataVar.name, intVar.Value);
                    }
                }
                else if (dataVar is IBaseTypeVar<float>)
                {
                    var floatVar = dataVar as IBaseTypeVar<float>;
                    if (!floatVar.HasRuntimeValue())
                    {
                        floatVar.Value = PlayerPrefs.GetFloat(dataVar.name, floatVar.Value);
                    }
                }
                else if (dataVar is IBaseTypeVar<bool>)
                {
                    var boolVar = dataVar as IBaseTypeVar<bool>;
                    if (!boolVar.HasRuntimeValue())
                    {
                        boolVar.Value = PlayerPrefs.GetInt(dataVar.name, boolVar.Value ? 1 : 0) == 1;
                    }
                }
                else if (dataVar is IBaseTypeVar<string>)
                {
                    var stringVar = dataVar as IBaseTypeVar<string>;
                    if (!stringVar.HasRuntimeValue())
                    {
                        stringVar.Value = PlayerPrefs.GetString(dataVar.name, stringVar.Value);
                    }
                }
            }
        }

        public void                             Save()
        {
            foreach (var dataVar in this._dataVars)
            {
                if (dataVar is IBaseTypeVar<int>)
                {
                    var intVar = dataVar as IBaseTypeVar<int>;
                    PlayerPrefs.SetInt(dataVar.name, intVar.Value);
                }
                else if (dataVar is IBaseTypeVar<float>)
                {
                    var floatVar = dataVar as IBaseTypeVar<float>;
                    PlayerPrefs.SetFloat(dataVar.name, floatVar.Value);
                }
                else if (dataVar is IBaseTypeVar<bool>)
                {
                    var boolVar = dataVar as IBaseTypeVar<bool>;
                    PlayerPrefs.SetInt(dataVar.name, boolVar.Value ? 1 : 0);
                }
                else if (dataVar is IBaseTypeVar<string>)
                {
                    var stringVar = dataVar as IBaseTypeVar<string>;
                    PlayerPrefs.SetString(dataVar.name, stringVar.Value);
                }
            }
            PlayerPrefs.Save();
        }
        #endregion
    }
}
