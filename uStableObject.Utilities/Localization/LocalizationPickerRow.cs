using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data.Localization;

namespace                                       uStableObject.UI.Localization
{
    public class                                LocalizationPickerRow : MonoBehaviour
    {
        #region Input Data
        [SerializeField] LocalizationManager    _locMan;
        [SerializeField] Text                   _name;
        [SerializeField] RawImage               _flag;
        #endregion

        #region Members
        LocalizationManager.LanguageData        _language;
        #endregion

        #region Properties
        public LocalizationManager.LanguageData Language { get { return (this._language); } }
        #endregion

        #region Trigger
        public void                             SetLanguageInfo(LocalizationManager.LanguageData language)
        {
            this._language = language;
            this._flag.texture = language._flag;
            this._flag.enabled = language._flag;
            this._name.text = language._name;
        }

        public void                             Select()
        {
            this._locMan.CurrentLanguage = this._language;
        }
        #endregion
    }
}
