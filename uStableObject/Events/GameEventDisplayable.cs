using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/GameEvent/Displayable", order = 5)]
    public class                GameEventDisplayable : GameEventBase<IDisplayable> { }
}
