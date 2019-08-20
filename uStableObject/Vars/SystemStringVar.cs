using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                               uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Settings/System String Var", order = 1)]
    public class                        SystemStringVar : StringVar, IGameEventListener<string>
    {
        [SerializeField] Modes          _mode;
        
        public void                     OnEnable()
        {
            this.Unregister(this);
            this.Register(this);
            if (this._mode == Modes.Resolution)
            {
                var currentRes = Screen.currentResolution;
                for (var i = Screen.resolutions.Length - 1; i >= 0; --i)
                {
                    if (Screen.resolutions[i].width <= currentRes.width
                        && Screen.resolutions[i].height <= currentRes.height)
                    {
                        if (Screen.resolutions[i].refreshRate != currentRes.refreshRate)
                        {
                            currentRes = Screen.resolutions[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                this._runtimeValue = GetResolutionText(currentRes);
                Debug.Log("Resolution: " + this._runtimeValue);
            }
            else
            {
                (this as IGameEventListener<string>).OnEventRaised(this.Value);
            }
        }

        void IGameEventListener<string>.OnEventRaised(string param)
        {
            switch (this._mode)
            {
                case Modes.Resolution:
                    {
                        int             resSeparatorIndex;
                        int             rateSeparatorIndex;
                        int             hzSeparatorIndex;
                        int             width;
                        int             height;
                        int             refreshRate;

                        resSeparatorIndex = param.IndexOf('x');
                        rateSeparatorIndex = param.IndexOf('@');
                        hzSeparatorIndex = param.IndexOf("Hz");
                        if (int.TryParse(param.Substring(0, resSeparatorIndex), out width)
                            && int.TryParse(param.Substring(resSeparatorIndex + 1, rateSeparatorIndex - (resSeparatorIndex + 1)), out height)
                            && int.TryParse(param.Substring(rateSeparatorIndex + 1, hzSeparatorIndex - (rateSeparatorIndex + 1)), out refreshRate))
                        {
                            PlayerPrefs.SetInt("Screenmanager Resolution Width", width);
                            PlayerPrefs.SetInt("Screenmanager Resolution Height", height);
                            Screen.SetResolution(width, height, Screen.fullScreen, refreshRate);
                        }
                        else
                        {
                            Debug.LogError("Trying to set invalid resolution: ");
                        }
                    }
                    break;
            }
        }

        public static string            GetResolutionText(Resolution res)
        {
            return (res.width + "x" + res.height + "@" + res.refreshRate + "Hz");
        }

        public enum                     Modes
        {
            Resolution
        }
    }
}
