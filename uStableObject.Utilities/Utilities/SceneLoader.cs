using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                SceneLoader : MonoBehaviour
    {
        #region Input Data
        [SerializeField] bool                   _verbose;
        [SerializeField] string                 _sceneName;
        [SerializeField] Object                 _scene;
        [SerializeField] LoadSceneMode          _mode;
        [SerializeField] ThreadPriority         _priorityMode = ThreadPriority.Normal;
        [SerializeField] UnityEvent             _onBeginLoad;
        [SerializeField] UnityEvent             _onBeginUnload;
        [SerializeField] UnityEventTypes.Float  _onLoadingProgress;
        [SerializeField] UnityEvent             _onFinishedLoad;
        [SerializeField] UnityEvent             _onFinishedUnload;
        #endregion

        #region Members
        States                                  _state = States.Unloaded;
        Scene                                   _loadedScene;
        AsyncOperation                          _activeLoading;
        #endregion

        #region Unity
        void                                    Awake()
        {
            //this._verbose = true;
        }

        void                                    Update()
        {
            if (this._activeLoading != null)
            {
                this._onLoadingProgress.Invoke(this._activeLoading.progress);
            }
        }

        void                                    OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSeneUnloaded;
            this._activeLoading = null;
        }
        #endregion

        #region Triggers
        public void                             PreLoadScene()
        {
            if (this._verbose)
            {
                Debug.Log(this.name + " SceneLoader.PreLoadScene #state=" + this._state + " #activeLoading" + this._activeLoading);
            }
            if (this._state < States.Loading && this._activeLoading == null)
            {
                this._state = States.Loading;
                Application.backgroundLoadingPriority = this._priorityMode;
                this._activeLoading = SceneManager.LoadSceneAsync(this._sceneName, this._mode);
                this._activeLoading.allowSceneActivation = false;
            }
        }

        public void                             LoadScene()
        {
            if (this._verbose)
            {
                Debug.Log(this.name + " SceneLoader.LoadScene #state=" + this._state + " #activeLoading" + this._activeLoading);
            }
            if (this._state < States.Loading)
            {
                this._state = States.Loading;
                if (!this._loadedScene.isLoaded)
                {
                    SceneManager.sceneLoaded -= OnSceneLoaded;
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    this._onBeginLoad.Invoke();
                    Application.backgroundLoadingPriority = this._priorityMode;
                    this._onLoadingProgress.Invoke(0);
                    this._activeLoading = SceneManager.LoadSceneAsync(this._sceneName, this._mode);
                    this.enabled = true;
                }
                else
                {
                    this.OnSceneLoaded(this._loadedScene,this._mode);
                }
            }
            else if (this._state == States.Loading && this._activeLoading != null)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.sceneLoaded += OnSceneLoaded;
                this._onBeginLoad.Invoke();
                this._onLoadingProgress.Invoke(0);
                this.enabled = true;
                this._activeLoading.allowSceneActivation = true;
            }
        }

        public void                             UnloadScene()
        {
            if (this._verbose)
            {
                Debug.Log(this.name + " SceneLoader.UnloadScene #state=" + this._state + " #activeLoading" + this._activeLoading);
            }
            if (this._state == States.Loaded)
            {
                if (this._loadedScene.isLoaded)
                {
                    this._state = States.Unloaded;
                    this._onBeginUnload.Invoke();
                    Application.backgroundLoadingPriority = this._priorityMode;
                    this._onLoadingProgress.Invoke(0);
                    this._activeLoading = SceneManager.UnloadSceneAsync(this._sceneName);
                    this.enabled = true;
                }
            }
            else if (this._state == States.Loading)
            {
                this._state = States.Unloading;
            }
        }

        public void                             ResetState()
        {
            this._state = States.Unloaded;
        }
        #endregion

        #region Callbacks
        void                                    OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (this._verbose)
            {
                Debug.Log(this.name + " SceneLoader.OnSceneLoaded #state=" + this._state + " #activeLoading" + this._activeLoading);
            }
            if (arg0.name == this._sceneName)
            {
                if (this._state < States.Loading)
                {
                    this._state = States.Loaded;
                    this.UnloadScene();
                }
                else
                {
                    this._state = States.Loaded;
                    this._loadedScene = arg0;
                    this._activeLoading = null;
                    SceneManager.sceneLoaded -= OnSceneLoaded;
                    SceneManager.sceneUnloaded -= OnSeneUnloaded;
                    SceneManager.sceneUnloaded += OnSeneUnloaded;
                    this._onLoadingProgress.Invoke(1);
                    this._onFinishedLoad.Invoke();
                }
            }
        }

        void                                    OnSeneUnloaded(Scene arg0)
        {
            if (this._verbose)
            {
                Debug.Log(this.name + " SceneLoader.OnSeneUnloaded #state=" + this._state + " #activeLoading" + this._activeLoading);
            }
            if (arg0.name == this._loadedScene.name)
            {
                this._state = States.Unloaded;
                this._loadedScene = default;
                this._activeLoading = null;
                SceneManager.sceneUnloaded -= OnSeneUnloaded;
                this._onFinishedUnload.Invoke();
            }
        }
        #endregion

        #region Data Types
        public enum                             States
        {
            Unloaded, Unloading, Loading, Loaded
        }
        #endregion
    }
}
