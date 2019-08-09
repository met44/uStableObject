using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uStableObject.Utilities;

namespace               uStableObject.Utilities
{
    public class        GoDespawn : MonoBehaviour
    {
        public void     DespawnNow()
        {
            GoPool.Despawn(this.gameObject);
        }
    }
}
