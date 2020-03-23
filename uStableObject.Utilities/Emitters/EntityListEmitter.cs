using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                   uStableObject.Utilities
{
    public class            EntityListEmitter : DataEmitterBase<IEntityList, UnityEventTypes.EntityList>
    {
        #region Triggers
        public void         Emit(EntityListVar val)
        {
            base.Emit(val);
        }

        public void         Memorize(EntityListVar val)
        {
            base.Memorize(val);
        }
        #endregion
    }
}