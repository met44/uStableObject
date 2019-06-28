using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;
using uStableObject.Utilities;

namespace                               uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Settings/System Int Var", order = 1)]
    public class                        SystemIntVar : IntVar, IGameEventListenerBase<int>
    {
        [SerializeField] Modes          _mode;

        void                            OnEnable()
        {
            this.Register(this);
            (this as IGameEventListenerBase<int>).OnEventRaised(this.Value);
        }

        void IGameEventListenerBase<int>.OnEventRaised(int param)
        {
            switch (this._mode)
            {
                case Modes.TargetFrameRate: Application.targetFrameRate = param; break;
                case Modes.FullScreenMode:
                    {
                        PlayerPrefs.SetInt("Screenmanager Fullscreen mode", param);
                        Screen.fullScreenMode = (FullScreenMode)param;
                    }
                    break;
                case Modes.QualityLevel:
                    {
                        PlayerPrefs.SetInt("UnityGraphicsQuality", param);
                        QualitySettings.SetQualityLevel(param, true);
                    }
                    break;
                case Modes.Monitor:
                    {
                    }
                    break;
            }
        }

        public enum                     Modes
        {
            TargetFrameRate, FullScreenMode, QualityLevel, Monitor
        }
    }
}
