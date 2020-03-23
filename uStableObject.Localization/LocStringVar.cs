using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uStableObject.Data;
using uStableObject.Data.Localization;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/LocVarString", order = 2)]
    public class                            LocStringVar : StringVar, ISerializationCallbackReceiver
    {
        #region Input Data
        [SerializeField] LocalizationVar    _locVar;
        #endregion

        #region Members
        LocalizationVar                     _runtimeLocVar;
        #endregion

        #region Properties
        public override string              Value
        {
            get
            {
                return (this._runtimeLocVar?.LocalizedText ?? this._runtimeValue);
            }
        }

        public LocalizationVar              LocVar
        {
            get
            {
                return (this._runtimeLocVar);
            }
            set
            {
                this._runtimeLocVar = value;
            }
        }
        #endregion

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() { this._runtimeLocVar = this._locVar; }
    }
}
