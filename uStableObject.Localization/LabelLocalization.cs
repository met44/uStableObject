using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities.Converters;

namespace                                   uStableObject.Data.Localization
{
    public class                            LabelLocalization : MonoBehaviour
    {
        #region Input Data
        [SerializeField] bool               _ignore;
        [SerializeField] Text               _label;
        [SerializeField] LocalizationVar    _localization;
        #endregion

        #region Properties
        public uint                         Version      { get; set; }
        public bool                         Ignore       { get { return (this._ignore); } }
        public LocalizationVar              Localization { get { return (this._localization); } }
        #endregion

        #region Unity
        void                                OnEnable()
        {
            if (!this.Ignore && this.Version != this._localization.Version)
            {
                this._label.text = this._localization.LocalizedText;
                this.Version = this._localization.Version;
            }
        }
        #endregion

#if UNITY_EDITOR
        void                                Reset()
        {
            this._label = this.GetComponent<Text>();
        }

        [ContextMenu("Create LocalizationVar")]
        public void                         CreateLocAsset()
        {
            string name = this._label.text.Replace(" ", "_").Replace("?", "").Replace("!", "").Replace(".", "");
            if (name.Contains("\n"))
            {
                name = name.Substring(0, name.IndexOf("\n"));
            }
            name = "Localization - " + name;
            string originalText = this._label.text;
            string hint = "";

            if (this._label.GetComponentInParent<Button>())
            {
                hint = "UI Button";
            }
            else if (this._label.name.Contains("Title")
                     || this._label.transform.parent.name.Contains("Title"))
            {
                hint = "UI Title";
            }
            this._localization = LocalizationManager.GetOrCreateLocAsset(name, hint, originalText, false); ;
            UnityEditor.EditorUtility.SetDirty(this);
            //UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("Create LocalizationVar", true)]
        public bool                         CreateLocAssetValidation()
        {
            return (!this._localization && !this._ignore
                    && !this.GetComponent<IntValueStringConverter>()
                    && !this.GetComponent<GameVarWatcherString>());
        }
#endif
    }
}
