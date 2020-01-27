using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace                               uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Input/Input Action", order = 3)]
    public class                        InputAction : ScriptableObject
    {
        #region Input Data
        [SerializeField] InputStack     _stack;
        [SerializeField] Types          _type = Types.KeyDown;
        [SerializeField] int            _mouseButtonNum;
        [SerializeField] Localized      _azertyData;
        [SerializeField] Localized      _qwertyData;
        [SerializeField] bool           _allowOverUI;
        #endregion

        #region Members
        public bool                     Locked { get; private set; }
        public KeyCode                  KeyboardKey { get; private set; }
        public KeyCode                  PositiveAxisKeyboardKey { get; private set; }
        public KeyCode                  NegativeAxisKeyboardKey { get; private set; }
        float                           _axisValue;
        float                           _axisVelocity;
        #endregion

        #region Triggers
        public void                     ToggleKeyboardLocalization(bool azerty)
        {
            Localized                   localizedKeyboardSetup;

            if (azerty)
            {
                localizedKeyboardSetup = this._azertyData;
            }
            else
            {
                localizedKeyboardSetup = this._qwertyData;
            }
            this.KeyboardKey = localizedKeyboardSetup.KeyboardKey;
            this.PositiveAxisKeyboardKey = localizedKeyboardSetup.PositiveAxisKeyboardKey;
            this.NegativeAxisKeyboardKey = localizedKeyboardSetup.NegativeAxisKeyboardKey;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void                     Register(System.Action callback)
        {
            this._stack.RegisterActionCallback(this, callback);
        }

        public void                     Unregister(System.Action callback)
        {
            this._stack.UnregisterActionCallback(this, callback);
        }

        // unregister without errors, use for action that dont need absolute consistency (ie, army hover may not be registered if occuring while picking another amry orders)
        public void                     TryUnregister(System.Action callback)
        {
            this._stack.TryUnregisterActionCallback(this, callback);
        }

        public void                     Trigger()
        {
            this._stack.TriggerAction(this);
        }

        public bool                     Check()
        {
            if (!this.Locked)
            {
                if (!InputStack.OverUI || this._allowOverUI)
                {
                    switch (this._type)
                    {
                        case Types.MouseDown: return (Input.GetMouseButtonDown(this._mouseButtonNum));
                        case Types.Mouse: return (Input.GetMouseButton(this._mouseButtonNum));
                        case Types.MouseUp: return (Input.GetMouseButtonUp(this._mouseButtonNum));
                        case Types.KeyDown: return (Input.GetKeyDown(this.KeyboardKey));
                        case Types.Key: return (Input.GetKey(this.KeyboardKey));
                        case Types.KeyUp: return (Input.GetKeyUp(this.KeyboardKey));
                        case Types.KeyDownUp: return (Input.GetKeyDown(this.KeyboardKey) || Input.GetKeyUp(this.KeyboardKey));
                        case Types.Axis: return (this.AxisValue() != 0);
                    }
                }
            }
            return (false);
        }

        public float                    AxisValue()
        {
            float                       axisTargetValue;

            if (this.Locked)
            {
                return (0);
            }
            if (Input.GetKey(this.PositiveAxisKeyboardKey))
            {
                axisTargetValue = 1;
            }
            else if (Input.GetKey(this.NegativeAxisKeyboardKey))
            {
                axisTargetValue = -1;
            }
            else
            {
                this._axisValue = 0;
                this._axisVelocity = 0;
                return (this._axisValue);
            }
            this._axisValue = Mathf.SmoothDamp(this._axisValue, axisTargetValue, ref this._axisVelocity, 0.4f);
            return (this._axisValue);
        }

        public void                     Lock()
        {
            this.Locked = true;
        }

        public void                     Unlock()
        {
            this.Locked = false;
        }
        #endregion

        #region Data Types
        public enum                     Types
        {
            MouseDown, Mouse, MouseUp, KeyDown, Key, KeyUp, Axis, KeyDownUp
        }

        [System.Serializable]
        public struct                   Localized
        {
            public KeyCode              KeyboardKey;
            public KeyCode              PositiveAxisKeyboardKey;
            public KeyCode              NegativeAxisKeyboardKey;
        }
        #endregion
    }
}
