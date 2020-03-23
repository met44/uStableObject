using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                           uStableObject.EditorX
{
    public class                    EditorDebugBreak : MonoBehaviour
    {
        #region Input Data
        [SerializeField] KeyCode    _breakKey = KeyCode.F4;
        #endregion

        #region Input Data
        void                        Update()
        {
            if (Input.GetKeyDown(this._breakKey))
            {
                Debug.Break();
            }
        }
        #endregion
    }
}
