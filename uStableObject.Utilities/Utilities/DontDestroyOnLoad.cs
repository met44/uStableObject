using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace                   uStableObject.Utilities
{
    public class            DontDestroyOnLoad : MonoBehaviour
    {
        public void         Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
