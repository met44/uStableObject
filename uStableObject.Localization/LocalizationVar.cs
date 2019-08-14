using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace                               uStableObject.Data.Localization
{
    [CreateAssetMenu(menuName = "uStableObject/Localization/LocalizationVar", order = 2)]
    public class                        LocalizationVar : ScriptableObject
    {
        #region Input Data
        [SerializeField] uint           _id;
        [SerializeField] string         _hint;
        [Multiline]
        [SerializeField] string         _original;
        #endregion

        #region Members
        [System.NonSerialized] string   _text;
        #endregion

        #region Properties
        public uint                     Version             { get; set; }
        public uint                     Id                  { get { return (this._id); } set { this._id = value; } }
        public string                   Hint                { get { return (this._hint); } set { this._hint = value; } }
        public string                   LocalizedTextRaw    { get { return (this._text); } }
        public string                   LocalizedText       { get { return (string.IsNullOrEmpty(this._text) ? this.Original : this._text); }
                                                              set { this._text = value; if (string.IsNullOrEmpty(value)) this.Version = 0; else ++this.Version; } }
        public string                   Original            { get { return (this._original); } set { this._original = value; } }
        #endregion

#if UNITY_EDITOR
        [ContextMenu("Select Using Labels In Scene")]
        public void                         SelectUsingLabelInScene()
        {
            List<GameObject> matchingObjects = new List<GameObject>();
            var scene = SceneManager.GetActiveScene();
            foreach (var obj in scene.GetRootGameObjects())
            {
                var llocs = obj.GetComponentsInChildren<LabelLocalization>(true);
                foreach (var lloc in llocs)
                {
                    if (lloc.Localization == this)
                    {
                        matchingObjects.Add(lloc.gameObject);
                    }
                }
            }
            if (matchingObjects.Count > 0)
            {
                UnityEditor.Selection.objects = matchingObjects.ToArray();
            }
        }

        public void                     UpdateData(string name, string hint, string originalText)
        {
            bool                        changed = false;

            if (this.name != name)
            {
                changed = true;
                this.name = name;
            }
            if (this._hint != hint)
            {
                changed = true;
                this._hint = hint;
            }
            if (this._original != originalText)
            {
                changed = true;
                this._original = originalText;
            }
            if (changed)
            {
                LocalizationManager.BumpVersion();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
