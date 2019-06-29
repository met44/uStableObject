using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data.Localization;

namespace                                       uStableObject.UI.Localization
{
    public class                                LocalizationPicker : MonoBehaviour
    {
        #region Input Data
        [SerializeField] LocalizationManager    _locMan = null;
        [SerializeField] RawImage               _currentLanguageFlag = null;
        [SerializeField] LocalizationPickerRow  _rowPrefab = null;
        [SerializeField] LayoutGroup            _contentRoot = null;
        #endregion
        
        #region Members
        List<LocalizationPickerRow>             _instances = new List<LocalizationPickerRow>();
        #endregion

        #region Unity
        void                                    OnEnable()
        {
            this.InitCurrentLanguageFlag();
            this.ShowAvailableLanguages();
        }
        #endregion

        #region Trigger
        public void                             InitCurrentLanguageFlag()
        {
            if (this._locMan.CurrentLanguage != null && !string.IsNullOrEmpty(this._locMan.CurrentLanguage._name))
            {
                this._currentLanguageFlag.texture = this._locMan.CurrentLanguage._flag;
            }
        }

        public void                             ShowAvailableLanguages()
        {
            int                                 i = 0;
            bool                                changed = false;

            foreach (var language in this._locMan.AvailableLanguages)
            {
                LocalizationPickerRow           instance = null;

                foreach (var instanceTmp in this._instances)
                {
                    if (instanceTmp.Language == language)
                    {
                        instance = instanceTmp;
                        break;
                    }
                }
                if (!instance)
                {
                    changed = true;
                    instance = Instantiate<LocalizationPickerRow>(this._rowPrefab, this._contentRoot.transform);
                    this._instances.Add(instance);
                }
                instance.transform.SetSiblingIndex(i);
                instance.SetLanguageInfo(language);
                ++i;
            }
            for (i = this._instances.Count - 1; i >= 0; --i)
            {
                bool                            found = false;

                foreach (var language in this._locMan.AvailableLanguages)
                {
                    if (this._instances[i].Language == language)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    changed = true;
                    DestroyImmediate(this._instances[i].gameObject);
                    this._instances.RemoveAt(i);
                }
            }
            if (changed)
            {
                this.ResizeScrollView();
                this._contentRoot.enabled = true;
                LayoutRebuilder.ForceRebuildLayoutImmediate(this._contentRoot.transform as RectTransform);
                this._contentRoot.enabled = false;
            }
            if (!this._contentRoot.gameObject.activeSelf)
            {
                this._contentRoot.gameObject.SetActive(true);
            }
        }
        #endregion

        #region Helpers
        void                                    ResizeScrollView()
        {
            RectTransform rt = this._contentRoot.transform as RectTransform;
            float rowheight = (this._rowPrefab.transform as RectTransform).rect.height;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, this._instances.Count * rowheight);
        }
        #endregion
    }
}
