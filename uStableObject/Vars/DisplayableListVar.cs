using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                       uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Displayable List", order = 2)]
    public abstract class                       DisplaybleListVar : ScriptableObject, IDisplayableList
    {
        public abstract IEnumerable<IDisplayable>    Entities { get; }
    }
}
