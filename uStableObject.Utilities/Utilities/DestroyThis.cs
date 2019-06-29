using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                   uStableObject.Utilities
{
    //helper to trigger from unity events
    public class            DestroyThis : MonoBehaviour
    {
        public void         DestroyNow()
        {
            Destroy(this.gameObject);
        }
    }
}
