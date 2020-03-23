using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                   uStableObject.Utilities
{
    //helper to trigger from unity events
    public class            UnsetParent : MonoBehaviour
    {
        public void         UnsetParentNow()
        {
            this.transform.parent = null;
        }
    }
}
