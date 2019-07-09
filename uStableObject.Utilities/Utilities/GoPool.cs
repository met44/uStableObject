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
                if (instance is Component)
                {
                    (instance as Component).transform.SetParent(parent);
                    (instance as Component).transform.SetPositionAndRotation(position, rotation);
                }
                else if (instance is GameObject)
                {
                    (instance as GameObject).transform.SetParent(parent);
                    (instance as GameObject).transform.SetPositionAndRotation(position, rotation);
                }
            }
            return (instance);
        }

        public static T                 Spawn<T>(T source, Transform parent = null) where T : Object
        {
            T                           instance;
            Storage                     storage;

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
                if (instance is Component)
                {
                    (instance as Component).transform.SetParent(parent);
                }
                else if (instance is GameObject)
                {
                    (instance as GameObject).transform.SetParent(parent);
                }
            }
            return (instance);
        }

        public static void              Despawn<T>(T instance) where T : Object
        {
            Storage                     storage;

            if (instance is Component)
            {
                (instance as Component).gameObject.SetActive(false);
            }
            else if (instance is GameObject)
            {
                (instance as GameObject).SetActive(false);
            }
            if (Instance._spawned.TryGetValue(instance, out storage))
            {
                storage.Store(instance);
            }
            else
            {
                Destroy(instance);
            }
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
