using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace                                       uStableObject.Data.Localization
{
    [CreateAssetMenu(menuName = "uStableObject/Localization/LocalizationManager", order = 2)]
    public class                                LocalizationManager : StringVar
    {
        #region Input Data
        [SerializeField] StringVar              _gameVersion = null;
        [SerializeField] int                    _version = 0;
        [SerializeField] string                 _languageKey = "";
        [SerializeField] SystemLanguage         _defaultLanguage = SystemLanguage.English;
        [SerializeField] Texture2D              _defaultLanguageFlag = null;
        [SerializeField] List<LocalizationVar>  _localizationVars = null;
        #endregion

        #region Members
        List<string>                            _localizationDirs = new List<string>();
        LanguageData                            _currentLanguage;
        bool                                    _cancelGoogleTranslate;
        #endregion

        #region Properties
        public override string                  Value //current language
        {
            get
            {
                if (Application.isPlaying)
                {
                    return (!string.IsNullOrEmpty(this._runtimeValue) ? this._runtimeValue : Application.systemLanguage.ToString());
                }
                else
                {
                    return (!string.IsNullOrEmpty(this._value) ? this._value : Application.systemLanguage.ToString());
                }
            }
            set
            {
                if (this._runtimeValue != value)
                {
                    this._runtimeValue = value;
                    foreach (var lang in this.AvailableLanguages)
                    {
                        if (lang._name == value)
                        {
                            this.CurrentLanguage = lang;
                            this.Raise(value);
                            break;
                        }
                    }
                }
            }
        }
        
        public LanguageData                     CurrentLanguage
        {
            get
            {
                return (this._currentLanguage);
            }
            set
            {
                this._currentLanguage = value;
                this._runtimeValue = this._currentLanguage._name;
                this.ImportCurrentLanguageTranslationData();
                this.Raise(this._runtimeValue);
            }
        }
        public List<LanguageData>               AvailableLanguages { get; private set; }
        public List<LocalizationVar>            LocalizationVars { get { return (this._localizationVars); } }
        public int                              ExportedVersion { get { return (File.Exists(this.DefaultLanguageFilePath) ? PlayerPrefs.GetInt("LocalizationVersion", -1) : -1) ; } }
#if UNITY_EDITOR
        public string                           DefaultLocPath { get { return ("./Assets/Data/Localization/Languages/"); } }
#else
        public string                           DefaultLocPath { get { return ("./Localization/"); } }
#endif
        public string                           DefaultLanguageFilePath { get { return (this.DefaultLocPath + this._defaultLanguage + "/" + this._defaultLanguage + ".json") ; } }
        public string                           CurrentLanguagePath { get { return (this._currentLanguage._path) ; } }
        public string                           CurrentLanguageJsonFilePath { get { return (this._currentLanguage._jsonPath) ; } }
        #endregion

        #region Unity
        void                                    OnEnable()
        {
            this._currentLanguage = null;
            if (!this._localizationDirs.Contains(this.DefaultLocPath))
            {
                this._localizationDirs.Add(this.DefaultLocPath);
            }
            if (this.ExportedVersion != this._version)
            {
                this.ExportDefaultLanguageTranslationData();
            }
            this.AvailableLanguages = new List<LanguageData>();
            this.RefreshAvailableLanguages();
        }

        void                                    OnDisable()
        {
            this.AvailableLanguages = new List<LanguageData>();
            this._localizationDirs = new List<string>();
            this._currentLanguage = null;
        }
        #endregion

        #region Triggers
        public void                             AddLocalizationDir(string dir)
        {
            if (!dir.EndsWith("" + Path.DirectorySeparatorChar))
            {
                dir += Path.DirectorySeparatorChar;
            }
            if (!this._localizationDirs.Contains(dir))
            {
                this._localizationDirs.Add(dir);
            }
        }

        public void                             RemoveLocalizationDir(string dir)
        {
            if (!dir.EndsWith("" + Path.DirectorySeparatorChar))
            {
                dir += Path.DirectorySeparatorChar;
            }
            this._localizationDirs.Remove(dir);
        }

        [ContextMenu("Refresh Available Languages")]
        public void                             RefreshAvailableLanguages()
        {
            this.AvailableLanguages.Clear();
            if (this._localizationDirs.Count == 0)
            {
                this._localizationDirs.Add(this.DefaultLocPath);
            }
            foreach (var localizationDir in this._localizationDirs)
            {
                if (Directory.Exists(localizationDir))
                {
                    var dirs = Directory.EnumerateDirectories(localizationDir);
                    foreach (var dir in dirs)
                    {
                        try
                        {
                            var languageName = Path.GetFileName(dir);
                            var jsonPath = Path.Combine(dir, languageName + ".json");
                            var flagImagePath = Path.Combine(dir, languageName + ".png");
                            bool isDefaultLanguage = languageName == this._defaultLanguage.ToString();
                            if (File.Exists(jsonPath)
                                && (File.Exists(flagImagePath) || isDefaultLanguage))
                            {
                                Texture2D               flagImage;

                                if (isDefaultLanguage)
                                {
                                    flagImage = this._defaultLanguageFlag;
                                }
                                else
                                {
                                    flagImage = new Texture2D(2, 2, TextureFormat.BGRA32, false);
                                    flagImage.LoadImage(File.ReadAllBytes(flagImagePath));
                                }
                                LanguageData language = new LanguageData()
                                {
                                    _name = languageName,
                                    _path = dir,
                                    _jsonPath = jsonPath,
                                    _flag = flagImage
                                };
                                this.AvailableLanguages.Add(language);
                                if (this._currentLanguage == null && languageName == this.Value)
                                {
                                    this.CurrentLanguage = language;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }
                }
                else
                {
                    Debug.LogError("Trying to fetch translations from a non existing folder: " + localizationDir);
                }
            }
        }
        
        [ContextMenu("Export Default Language")]
        public void                             ExportDefaultLanguageTranslationData()
        {
            try
            {
                var exportData = this.GetDefaultLanguageExportData();
                string json = JsonUtility.ToJson(exportData, true);
                if (!Directory.Exists(this.DefaultLocPath))
                {
                    Directory.CreateDirectory(this.DefaultLocPath);
                }
                if (!Directory.Exists(this.DefaultLocPath + this._defaultLanguage))
                {
                    Directory.CreateDirectory(this.DefaultLocPath + this._defaultLanguage);
                }
                Debug.Log("Writting localization data to: " + this.DefaultLanguageFilePath + "...");
                File.WriteAllText(this.DefaultLanguageFilePath, json);
                Debug.Log("Done !");
                PlayerPrefs.SetInt("LocalizationVersion", this._version);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        
        [ContextMenu("Export Current Language")]
        public void                             ExportCurrentLanguageTranslationData()
        {
            this.ExportCurrentLanguageTranslationDataTo(this.CurrentLanguagePath);
        }

        public void                             ExportCurrentLanguageTranslationDataTo(string dir)
        {
            try
            {
                var exportData = this.GetCurrentLanguageExportData();
                string json = JsonUtility.ToJson(exportData, true);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                dir = Path.Combine(dir, this.Value);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string fullpath = Path.Combine(dir, this.Value + ".json");
                Debug.Log("Writting localization data to: " + fullpath + "...");
                File.WriteAllText(fullpath, json);
                Debug.Log("Done !");
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        
        [ContextMenu("Import Current Language")]
        public bool                             ImportCurrentLanguageTranslationData()
        {
            try
            {
                Debug.Log("Reading localization data from: " + this.CurrentLanguageJsonFilePath + "...");
                var importData = File.ReadAllText(this.CurrentLanguageJsonFilePath);
                var data = JsonUtility.FromJson<LocExportData>(importData);
                this._runtimeValue = data.Language;
                int valueSet = 0;
                foreach (var entry in data.Entries)
                {
                    try
                    {
                        var lloc = this._localizationVars.Find(lv => lv.Id == entry.Id);
                        if (lloc != null/* && !string.IsNullOrEmpty(entry.LocalizedText)*/)
                        {
                            lloc.LocalizedText = entry.LocalizedText;
                            ++valueSet;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
                if (valueSet < this._localizationVars.Count)
                {
                    foreach (var lloc in this._localizationVars)
                    {
                        try
                        {
                            if (!data.Entries.Any(e => e.Id == lloc.Id))
                            {
                                lloc.LocalizedText = null;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }
                }
                Debug.Log("Done ! (" + valueSet + "/" + this._localizationVars.Count + ")");
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return (false);
            }
            return (true);
        }

        [ContextMenu("Load Current Language from google")]
        public async void                       LoadCurrentLanguageFromGoogle()
        {
            foreach (var loc in this._localizationVars)
            {
                if (string.IsNullOrEmpty(loc.LocalizedTextRaw))
                {
                    await this.TranslateToCurrentLanguage(loc);
                    //await Task.Delay(300);
                }
                if (this._cancelGoogleTranslate)
                {
                    this._cancelGoogleTranslate = false;
                    break;
                }
            }
        }
        [ContextMenu("Cancel Load Current Language from google")]
        public void                             CancelLoadCurrentLanguageFromGoogle()
        {
            this._cancelGoogleTranslate = true;
        }
        #endregion
        
        #region Helpers
        object                                  GetDefaultLanguageExportData()
        {
            return (new LocExportData
            {
                GameVersion = this._gameVersion.Value,
                LocalizationVersion = this._version,
                Language = this._defaultLanguage.ToString(),
                Entries = this._localizationVars.Select(loc => new LocExportData.Entry { Id = loc.Id, Hint = loc.Hint, Original = loc.Original, LocalizedText = null }).ToList()
            });
        }

        object                                  GetCurrentLanguageExportData()
        {
            return (new LocExportData
            {
                GameVersion = this._gameVersion.Value,
                LocalizationVersion = this._version,
                Language = this.Value,
                Entries = this._localizationVars.Select(loc => new LocExportData.Entry { Id = loc.Id, Hint = loc.Hint, Original = loc.Original, LocalizedText = loc.LocalizedTextRaw }).ToList()
            });
        }

        async Task                              TranslateToCurrentLanguage(LocalizationVar loc)
        {
            string langKey = this._languageKey.ToLower();
            string originalText = loc.Original;
            string url = string.Format ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                                        "en",
                                        langKey,
                                        UnityWebRequest.EscapeURL(originalText));
            var req = UnityWebRequest.Get(url);
            req.SetRequestHeader("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 " +
                                                "(KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
            await req.SendWebRequest();
            if (req.isNetworkError || req.isHttpError)
            {
                /*Debug.Log(req.GetResponseHeader("Retry-After"));*/
                Debug.LogError(req.error);
                var responseHeaders = req.GetResponseHeaders();
                foreach (var responH in responseHeaders)
                {
                    Debug.Log(responH.Key + ": " + responH.Value);
                }
                throw new System.Exception();
            }
            else
            {
                //Debug.Log(req.downloadHandler.text);

                loc.LocalizedText = this.ExtractTranslationFromJson(req.downloadHandler.text, langKey);
                Debug.Log(originalText + " => " + loc.LocalizedText);
            }
        }

        string                                  ExtractTranslationFromJson(string text, string langKey)
        {
            //https://www.codeproject.com/Articles/12711/Google-Translator
            string                              translation = "";

            // Extract text
            text = text.Substring(4);// index);
            text = text.Substring(0, text.LastIndexOf(']'));// index);

            // Get translated phrases
            string[] phrases = text.Split (new[] { "],[\"" }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; (i < phrases.Count()); ++i)
            {
                string translatedPhrase = phrases[i].Substring(0, phrases[i].IndexOf("\",\"")).Replace("\\n", "\n");
                translation += translatedPhrase;
            }

            // Fix up translation
            translation = translation.Trim();
            translation = translation.Replace(" ?", "?");
            translation = translation.Replace(" !", "!");
            translation = translation.Replace(" ,", ",");
            translation = translation.Replace(" .", ".");
            translation = translation.Replace(" ;", ";");
            translation = translation.Replace("\\u003c/ ", "</");
            translation = translation.Replace("\\u003c", "<");
            translation = translation.Replace("\\u003e", ">");
            return (translation);
        }
        #endregion

#if UNITY_EDITOR
        [ContextMenu("Fix ids")]
        public void                             FixIds()
        {
            for (var i = 0; i < this._localizationVars.Count; ++i)
            {
                this._localizationVars[i].Id = (uint)i + 1;
                UnityEditor.EditorUtility.SetDirty(this._localizationVars[i]);
            }
        }

        public static LocalizationVar           GetOrCreateLocAsset(string locName, string hint, string originalText, bool forceCreate)
        {
            LocalizationManager locMan = (LocalizationManager)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Data/Localization/_LocalizationManager.asset", typeof(LocalizationManager));
            while (locMan.LocalizationVars.Remove(null)) { }
            LocalizationVar loc = locMan.LocalizationVars.Find(l => /*l.Hint == hint && l.Original == originalText &&*/l &&  l.name == locName);
            uint nextID = locMan._localizationVars.Max(lv => lv.Id) + 1;

            if (loc != null)
            {
                if (forceCreate
                    || loc.Original != originalText)
                {
                    loc = null;
                    locName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Data/Localization/" + locName + ".asset");
                    locName = locName.Substring(locName.LastIndexOf('/') + 1);
                    locName = locName.Substring(0, locName.Length - 6);
                }
                else if (!loc.Hint.Contains(hint))
                {
                    loc.Hint += "; "  + hint;
                    LocalizationManager.BumpVersion();
                }
            }

            if (loc == null)
            {
                loc = ScriptableObject.CreateInstance<LocalizationVar>();
                loc.name = locName;
                loc.Id = nextID;
                loc.Original = originalText;
                loc.Hint = hint;
                UnityEditor.AssetDatabase.CreateAsset(loc, "Assets/Data/Localization/" + locName + ".asset");
                locMan._localizationVars.Add(loc);
                Debug.Log("Created LocVar: " + loc.name);
                LocalizationManager.BumpVersion();
                UnityEditor.EditorUtility.SetDirty(loc);
            }
            UnityEditor.EditorUtility.SetDirty(locMan);
            return (loc);
        }

        [ContextMenu("Bump version")]
        public static void                      BumpVersion()
        {
            LocalizationManager locMan = (LocalizationManager)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Data/Localization/_LocalizationManager.asset", typeof(LocalizationManager));
            ++locMan._version;
            UnityEditor.EditorUtility.SetDirty(locMan);
        }
#endif
        [System.Serializable]
        public class                            LanguageData
        {
            public string                       _name;
            public string                       _path;
            public string                       _jsonPath;
            public Texture                      _flag;
        }

        [System.Serializable]
        public class                            LocExportData
        {
            public string                       GameVersion;
            public int                          LocalizationVersion;
            public string                       Language;
            public List<Entry>                  Entries = new List<Entry>();

            [System.Serializable]
            public class                        Entry
            {
                public uint                     Id;
                public string                   Hint;
                public string                   LocalizedText;
                public string                   Original;
            }
        }
    }
}
