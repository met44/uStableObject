using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                   uStableObject
{
    [RequireComponent(typeof(Renderer))]
    public class            UniqueMat : MonoBehaviour
    {
        #region Unity
        void                Awake()
        {
            var mat = this.GetComponent<Renderer>().material;
        }
        #endregion
    }
}
