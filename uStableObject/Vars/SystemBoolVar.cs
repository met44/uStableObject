using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                               uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Settings/System Bool Var", order = 1)]
    public class                        SystemBoolVar : BoolVar, IGameEventListener<bool>
    {
        [SerializeField] Modes          _mode;

        public bool                     Editing { get; set; }

        private void                    OnEnable()
        {
            this.Register(this);
            if (this._mode == Modes.Fullscreen)
            {
                this.Value = Screen.fullScreen;
            }
            else
            {
                (this as IGameEventListener<bool>).OnEventRaised(this.Value);
            }
        }

        void IGameEventListener<bool>.OnEventRaised(bool param)
        {
            switch (this._mode)
            {
                case Modes.ConfineCursor: Cursor.lockState = param ? CursorLockMode.Confined : CursorLockMode.None; break;
                case Modes.Fullscreen: Screen.fullScreen = param; break;
            }
        }

        public enum                     Modes
        {
            ConfineCursor, Fullscreen
        }
    }
}
