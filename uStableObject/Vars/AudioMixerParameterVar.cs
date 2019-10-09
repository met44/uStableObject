using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                               uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Settings/AudioMixer Parameter Var", order = 1)]
    public class                        AudioMixerParameterVar : FloatVar, IGameEventListener<float>
    {
        #region Input Data
        [SerializeField] string         _parameter;
        [SerializeField] AudioMixer     _target;
        [SerializeField] Modes          _mode;
        [SerializeField] float          _scale = 20;
        [SerializeField] float          _offset;
        #endregion

        #region Properties
        public bool                     Editing { get; set; }
        #endregion

        #region Unity
        private void                    OnEnable()
        {
            this.Register(this);
            (this as IGameEventListener<float>).OnEventRaised(this.Value);
        }
        #endregion

        void IGameEventListener<float>.OnEventRaised(float param)
        {
            if (this._mode == Modes.Log)
            {
                param = Mathf.Log(param);
            }
            this._target.SetFloat(this._parameter, param * this._scale + this._offset);
        }

        #region Data Types
        public enum                     Modes
        {
            Log, Linear
        }
        #endregion
    }
}
