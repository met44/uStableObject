using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace                                       uStableObject.Data
{
    [System.Serializable]
    public static class                         SBinder<T> 
                                                where T : IObjectID
    {
        #region Members
        static Dictionary<uint, T>              _refs= new Dictionary<uint, T>();
        #endregion

        #region Triggers
        public static void                      Clear()
        {
            _refs.Clear();
        }

        //Check whether is either free or pointing to the right object
        public static bool                      CheckIDValidity(T obj)
        {
            T                                   existingRef = default(T);

            _refs.TryGetValue(obj.ID, out existingRef);
            return (obj.ID > 0 && (existingRef == null || existingRef.Equals(obj)));
        }

        public static uint                      GetAvailableID()
        {
            uint highestId = 0;
            foreach (var key in _refs.Keys)
            {
                if (key > highestId)
                {
                    highestId = key;
                }
            }
            return (highestId + 1);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void                      EnsureUniqueID(T obj)
        {
#if UNITY_EDITOR
            if (!CheckIDValidity(obj))
            {
                FieldInfo                       idField = null;

                var type = obj.GetType();
                while (idField == null)
                {
                    idField = type.GetField("_id", BindingFlags.Instance | BindingFlags.NonPublic);
                    type = type.BaseType;
                    if (type.BaseType == typeof(System.Object))
                    {
                        throw new System.Exception("_id field not found");
                    }
                }
                uint newId = GetAvailableID();
                idField.SetValue(obj, newId);
                if (obj is Object)
                {
                    UnityEditor.EditorUtility.SetDirty(obj as Object);
                }
            }
#endif
        }

        public static void                      Init(T obj)
        {
            if (_refs.ContainsKey(obj.ID))
            {
                _refs[obj.ID] = obj;
            }
            else
            {
                _refs.Add(obj.ID, obj);
            }
        }

        public static T                         Get(uint id)
        {
            if (_refs.TryGetValue(id, out T output))
            {
                return (output);
            }
            return (default(T));
        }
        #endregion
    }
}
