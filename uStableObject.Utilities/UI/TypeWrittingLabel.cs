using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;
using uStableObject.Utilities;
using System.Text;

namespace                                   uStableObject
{
    public class                            TypeWrittingLabel : MonoBehaviour
    {
        #region Input Data
        [SerializeField] StringVar          _value;
        [SerializeField] Text               _target;
        [SerializeField] bool               _disableAfter;
        [SerializeField] IntervalF          _typeWrittingSmoothTimeDelay;
        [SerializeField] UnityEvent         _onDisplayedValueChanged = null;
        [SerializeField] UnityEvent         _onFinishedDisplay;
        #endregion

        #region Members
        StringBuilder                       _stringBuilder = new StringBuilder();
        float _nextTypingTime;
        int                                 _displayedIndex;
        #endregion

        #region Properties
        public string                       DefaultValue { get; set; }
        #endregion

        #region Unity
        public void                         Update()
        {
            string targetValue = this._value ?? this.DefaultValue;
            if (this._displayedIndex != targetValue.Length)
            {
                if (this._nextTypingTime <= Time.realtimeSinceStartup)
                {
                    this._nextTypingTime = Time.realtimeSinceStartup + this._typeWrittingSmoothTimeDelay.Rand;
                    int newDisplayedIndex = this._displayedIndex + 1;
                    if (this._displayedIndex != newDisplayedIndex)
                    {
                        for (var i = this._displayedIndex; i < newDisplayedIndex; ++i)
                        {
                            this._stringBuilder.Replace(' ', targetValue[i], i, 1);
                        }
                        this._displayedIndex = newDisplayedIndex;
                        this._target.text = this._stringBuilder.ToString();
                        this._onDisplayedValueChanged.Invoke();
                    }
                }
            }
            else
            {
                if (this._disableAfter)
                {
                    this.enabled = false;
                }
                this._onFinishedDisplay.Invoke();
            }
        }
        #endregion

        #region Triggers
        public void                         ShowText(string text)
        {
            this.DefaultValue = text;
            this.Restart();
        }

        public void                         FinishDisplayNow()
        {
            string targetValue = this._value ?? this.DefaultValue;
            this._displayedIndex = targetValue.Length;
            this._target.text = targetValue;
            this._onDisplayedValueChanged.Invoke();
        }

        public void                         Restart()
        {
            var text = this._value ?? this.DefaultValue;
            this._displayedIndex = 0;
            this._stringBuilder.Clear();
            this._stringBuilder.EnsureCapacity(text.Length);
            this._stringBuilder.Append(' ', text.Length);
            for (var i = 0; i < text.Length; ++i)
            {
                if (text[i] == '\n')
                {
                    this._stringBuilder.Replace(' ', '\n', i, 1);
                }
            }
            this._target.text = this._stringBuilder.ToString();
            this.enabled = true;
        }
        #endregion

        #region Data Types
        #endregion
    }
}
