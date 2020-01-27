using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;
using uStableObject.Utilities;

namespace                                   uStableObject
{
    public class                            TypeWrittingLabelInt : MonoBehaviour
    {
        #region Input Data
        [SerializeField] IntVar             _value;
        [SerializeField] Text               _target;
        [SerializeField] int                _displayedValue;
        [SerializeField] bool               _disableAfter;
        [SerializeField] float              _typeWrittingSmoothTime = 1f;
        [SerializeField] UnityEvent         _onDisplayedValueChanged = null;
        #endregion

        #region Members
        float                               _typeWrittingVal = 0;
        float                               _typeWrittingVelocity = 0;
        #endregion

        #region Properties
        public int                          DefaultValue { get; set; }
        #endregion

        #region Unity
        public void                         Update()
        {
            int targetValue = this._value ?? this.DefaultValue;
            if (targetValue != this._displayedValue)
            {
                this._typeWrittingVal = Mathf.SmoothDamp(this._typeWrittingVal, targetValue, ref this._typeWrittingVelocity, this._typeWrittingSmoothTime);
                int newDisplayedValue = Mathf.RoundToInt(this._typeWrittingVal);
                if (this._displayedValue != newDisplayedValue)
                {
                    this._displayedValue = newDisplayedValue;
                    this._target.text = this._displayedValue.ToStringNonAlloc();
                    this._onDisplayedValueChanged.Invoke();
                }
            }
            else
            {
                this._typeWrittingVelocity = 0;
                if (this._disableAfter)
                {
                    this.enabled = false;
                }
            }
        }
        #endregion

        #region Triggers
        #endregion

        #region Data Types
        #endregion
    }
}
