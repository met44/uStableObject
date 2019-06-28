using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                               uStableObject
{
    public class                        AssetsPreloading : MonoBehaviour
    {
        [System.Serializable]
        public class                    ObjectArray
        {
            [SerializeField] Object[]   _assets;
            public Object[]             Asset { get { return (this._assets); } }
        }
      
        #region Input Data
        [SerializeField] ObjectArray[]  _assetsLists;
        #endregion

        #region Members
        void                            Awake()
        {
            int osef = 0;
            foreach (var list in this._assetsLists)
            {
                foreach (var asset in list.Asset)
                {
                    osef += asset.GetInstanceID();
                }
            }
        }
        #endregion
    }
}
