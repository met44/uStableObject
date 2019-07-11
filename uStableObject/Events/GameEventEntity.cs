using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/GameEvent/Entity", order = 5)]
    public class                GameEventEntity : GameEventBase<IEntity> { }
}
