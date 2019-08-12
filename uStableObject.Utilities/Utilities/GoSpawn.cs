using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uStableObject.Utilities;

namespace                               uStableObject.Utilities
{
    public class                        GoSpawn : MonoBehaviour
    {
        #region Input Data
        [SerializeField] GameObject     _prefab;
        [SerializeField] Transform      _parent;
        #endregion

        #region Triggers
        public void                     SpawnNow()
        {
            GoPool.Spawn(this._prefab, this.transform.position, this.transform.rotation, this._parent);
        }
        #endregion
    }
}
