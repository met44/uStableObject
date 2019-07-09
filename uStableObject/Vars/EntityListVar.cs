using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                       uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Entity List", order = 2)]
    public abstract class                       EntityListVar : GameEventData, IEntityListVar
    {
        public abstract IEnumerable<IEntity>    Entities { get; }
    }
}
