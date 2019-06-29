using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                   uStableObject.Utilities
{
    public class            QuitGame : MonoBehaviour
    {
        #region Triggers
        public void         QuitGameNow()
        {
            //todo cleanup steam stuff ?
            Application.Quit();
        }
        #endregion
    }
}
