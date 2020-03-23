using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                   uStableObject.Utilities
{
    //helper to trigger from unity events
    public class            InstantiateThis : MonoBehaviour
    {
        public void         InstantiateNow()
        {
            Instantiate(this.gameObject, this.transform.position, this.transform.rotation);
        }
    }
}
