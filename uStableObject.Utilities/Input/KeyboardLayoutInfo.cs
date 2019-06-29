using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using uStableObject.Data;

namespace                               uStableObject.Utilities
{
    [CreateAssetMenu(menuName = "uStableObject/Input/Keyboard Layout Info")]
    public class                        KeyboardLayoutInfo : ScriptableObject
    {
        #region Input Data
        [SerializeField] BoolVar        _azertyKeyboardLayout;
        [SerializeField] InputAction[]  _actions;
        #endregion

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr hwnd, IntPtr proccess);
        [DllImport("user32.dll")] static extern IntPtr GetKeyboardLayout(uint thread);
#endif
        [NonSerialized] bool            _initialized;
        [NonSerialized] bool            _azerty;

        #region Triggers
        public void                     OnEnable()
        {
            if (PlayerPrefs.HasKey(this._azertyKeyboardLayout.name))
            {
                this.CustomRefresh();
            }
            else
            {
                this.Refresh();
            }
        }
        #endregion

        #region Triggers
        [ContextMenu("Force Custom Refresh")]
        public void                     CustomRefresh()
        {
            if (Application.isPlaying)
            {
                //this._initialized = true;
                foreach (var action in this._actions)
                {
                    action.ToggleKeyboardLocalization(this._azertyKeyboardLayout.Value);
                }
            }
        }

        [ContextMenu("Force Auto Refresh")]
        public void                     Refresh()
        {
            if (Application.isPlaying)
            {
                if (this.CheckKeyboardLayoutChanged()
                    || !this._initialized)
                {
                    //this._initialized = true;
                    foreach (var action in this._actions)
                    {
                        action.ToggleKeyboardLocalization(this._azertyKeyboardLayout.Value);
                    }
                }
            }
        }
        #endregion
        
        #region Helpers
        bool                            CheckKeyboardLayoutChanged()
        {
            var layout = GetCurrentKeyboardLayout();
            Debug.Log("Keyboard Layout=" +  layout);
            bool azerty = !(layout == 1033);
            if (this._azertyKeyboardLayout.Value != azerty)
            {
                this._azertyKeyboardLayout.Value = azerty;
                return (true);
            }
            return (false);
        }

        int                             GetCurrentKeyboardLayout()
        {
            try
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                IntPtr foregroundWindow = GetForegroundWindow();
                uint foregroundProcess = GetWindowThreadProcessId(foregroundWindow, IntPtr.Zero);
                int keyboardLayout = GetKeyboardLayout(foregroundProcess).ToInt32() & 0xFFFF;
#else
                int keyboardLayout = 0;
#endif

                if (keyboardLayout == 0)
                {
                    // something has gone wrong - just assume English
                    keyboardLayout = 1033;
                }
                return keyboardLayout;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                // if something goes wrong - just assume English
                return 1033;
            }
        }
        #endregion
    }
}
