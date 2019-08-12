using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace                               uStableObject.Utilities
{
    public class                        GoPool : MonoBehaviour
    {
        #region Input Data
        #endregion

        #region Members
        Dictionary<GameObject, Storage> _storage = new Dictionary<GameObject, Storage>();
        Dictionary<GameObject, Storage> _spawned = new Dictionary<GameObject, Storage>();
        #endregion

        #region Singleton
        static GoPool                   _instance;
        static GoPool                   Instance
        {
            get
            {
                if (!_instance)
                {
                    var go = new GameObject("GoPool");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<GoPool>();
                }
                return (_instance);
            }
        }

        void                            Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        void                            OnDestroy()
        {
            if (_instance == null)
            {
                _instance = null;
                Destroy(this.gameObject);
            }
        }
        #endregion

        #region Triggers
        public static T                 Spawn<T>(T source, Vector3 position, Quaternion rotation, Transform parent = null) where T : Object
        {
            T                           instance;
            Storage                     storage;
            GameObject                  sourceGo;
            GameObject                  go;

            sourceGo = GetGameObject(source);
            if (!Instance._storage.TryGetValue(sourceGo, out storage))
            {
                storage = AutoPool<Storage>.Create();
                Instance._storage.Add(sourceGo, storage);
            }
            instance = storage.Get<T>();
            if (instance == null)
            {
                instance = Instantiate(source, position, rotation, parent);
                go = GetGameObject(instance);
            }
            else
            {
                go = GetGameObject(instance);
                go.transform.SetParent(parent);
                go.transform.SetPositionAndRotation(position, rotation);
                go.SetActive(true);
            }
            Instance._spawned.Add(go, storage);
            return (instance);
        }

        public static T                 Spawn<T>(T source, Transform parent = null) where T : Object
        {
            T                           instance;
            Storage                     storage;
            GameObject                  sourceGo;
            GameObject                  go;

            sourceGo = GetGameObject(source);
            if (!Instance._storage.TryGetValue(sourceGo, out storage))
            {
                storage = AutoPool<Storage>.Create();
                Instance._storage.Add(sourceGo, storage);
            }
            instance = storage.Get<T>();
            if (instance == null)
            {
                instance = Instantiate(source, parent);
                go = GetGameObject(instance);
            }
            else
            {
                go = GetGameObject(instance);
                go.transform.SetParent(parent);
                go.SetActive(true);
            }
            Instance._spawned.Add(go, storage);
            return (instance);
        }

        public static void              Despawn<T>(T instance) where T : Object
        {
            Storage                     storage;
            GameObject                  go;

            go = GetGameObject(instance);
            go.SetActive(false);
            if (Instance._spawned.TryGetValue(go, out storage))
            {
                Instance._spawned.Remove(go);
                storage.Store(go);
            }
            else
            {
                Destroy(go);
            }
        }
        #endregion

        #region Helpers
        static GameObject               GetGameObject<T>(T instance) where T : Object
        {
            GameObject                  go;

            if (instance is Component)
            {
                go = (instance as Component).gameObject;
            }
            else if (instance is GameObject)
            {
                go = instance as GameObject;
            }
            else
            {
                go = null;
                Debug.LogError("Wrong type of object, no gameobject to disable for " + instance.name);
            }
            return (go);
        }
        #endregion

        #region Data Types
        [System.Serializable]
        public class                    Storage
        {
            Queue<GameObject>           _pooled = new Queue<GameObject>();
            
            public T                    Get<T>() where T : Object
            {
                if (this._pooled.Count > 0)
                {
                    var instance = this._pooled.Dequeue();
                    return (instance.GetComponent<T>());
                }
                return (null);
            }
            
            public GameObject           Get()
            {
                if (this._pooled.Count > 0)
                {
                    var instance = this._pooled.Dequeue();
                    return (instance);
                }
                return (null);
            }

            public void                 Store(GameObject instance) 
            {
                this._pooled.Enqueue(instance);
            }
        }
        #endregion
    }
}
