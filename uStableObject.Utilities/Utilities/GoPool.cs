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
        Dictionary<object, Storage>     _storage = new Dictionary<object, Storage>();
        Dictionary<Object, Storage>     _spawned = new Dictionary<Object, Storage>();
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
            GameObject                  go;

            if (!Instance._storage.TryGetValue(source, out storage))
            {
                storage = AutoPool<Storage>.Create();
                Instance._storage.Add(source, storage);
            }
            instance = storage.Get<T>();
            if (instance == null)
            {
                instance = Instantiate(source, position, rotation, parent);
            }
            else
            {
                go = GetGameObject(instance);
                go.transform.SetParent(parent);
                go.transform.SetPositionAndRotation(position, rotation);
                go.SetActive(true);
            }
            Instance._spawned.Add(instance, storage);
            return (instance);
        }

        public static T                 Spawn<T>(T source, Transform parent = null) where T : Object
        {
            T                           instance;
            Storage                     storage;
            GameObject                  go;

            if (!Instance._storage.TryGetValue(source, out storage))
            {
                storage = AutoPool<Storage>.Create();
                Instance._storage.Add(source, storage);
            }
            instance = storage.Get<T>();
            if (instance == null)
            {
                instance = Instantiate(source, parent);
            }
            else
            {
                go = GetGameObject(instance);
                go.transform.SetParent(parent);
                go.SetActive(true);
            }
            Instance._spawned.Add(instance, storage);
            return (instance);
        }

        public static void              Despawn<T>(T instance) where T : Object
        {
            Storage                     storage;
            GameObject                  go;

            go = GetGameObject(instance);
            go.SetActive(false);
            if (Instance._spawned.TryGetValue(instance, out storage))
            {
                Instance._spawned.Remove(instance);
                storage.Store(instance);
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
            Queue                       _pooled = new Queue();
            
            public T                    Get<T>() where T : Object
            {
                if (this._pooled.Count > 0)
                {
                    return ((T)this._pooled.Dequeue());
                }
                return (null);
            }

            public void                 Store<T>(T instance) where T : Object
            {
                this._pooled.Enqueue(instance);
            }
        }
        #endregion
    }
}
