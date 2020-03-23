using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                       uStableObject.Data
{
    [System.Serializable]
    public class                                SProxy<T> 
                                                where T : IObjectID
    {
        #region Members
        [SerializeField] protected uint         _id;
        #endregion

        #region Properties
        public uint                             ID                  => this._id;
        private T                               Instance           { get; set; }
        public static implicit operator         T(SProxy<T> proxy)  => proxy.Instance == null ? (proxy.Instance = SBinder<T>.Get(proxy._id)) : proxy.Instance;

        //implicit cast version is sexier but unity wont serialize the generic version :/
        public static P                         Make<P>(T obj) where P : SProxy<T>, new()
        {
            return (new P() { _id = obj.ID, Instance = obj });
        }
        #endregion
    }
}