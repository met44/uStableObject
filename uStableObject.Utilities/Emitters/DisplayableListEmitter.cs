using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                   uStableObject.Utilities
{
    public class            DisplayableListEmitter : DataEmitterBase<IDisplayableList, UnityEventTypes.DisplayableList>
    {
        #region Triggers
        public void         Emit(DisplaybleListVar val)
        {
            base.Emit(val);
        }

        public void         Memorize(DisplaybleListVar val)
        {
            base.Memorize(val);
        }
        #endregion
    }
}