using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Utilities;

namespace                                       uStableObject.Utilities.Converters
{
    public class                                BoolValueBoolConverter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] Modes                  _mode = Modes.Invert;
        [SerializeField] UnityEventTypes.Bool   _ouputValue;
        #endregion

        #region Properties
        bool                                    Invert          { get { return ((this._mode & Modes.Invert) != 0); } }
        bool                                    PassOnlyIfFalse { get { return ((this._mode & Modes.PassOnlyIfFalse) != 0); } }
        bool                                    PassOnlyIfTrue  { get { return ((this._mode & Modes.PassOnlyIfTrue) != 0); } }
        #endregion

        #region Triggers
        public void                             ConvertValue(bool val)
        {
            if (this._mode > Modes.Invert)
            {
                if (this.PassOnlyIfFalse)
                {
                    if (val) return;
                }
                else if (this.PassOnlyIfTrue && !val)
                {
                    return;
                }
            }
            this._ouputValue.Invoke(val != this.Invert);
        }
        #endregion

        #region Data Types
        [System.Flags]
        public enum                             Modes
        {
            Invert = 1,
            PassOnlyIfFalse = 2,
            PassOnlyIfTrue = 4,
            PassOnlyIfFalseThenInvert = 3,
            PassOnlyIfTrueThenInvert = 5
        }
        #endregion
    }
}
