using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                           uStableObject
{
    public static partial class     UnityEventTypes
    {
        [System.Serializable]
        public class        GO : UnityEvent<GameObject> { }
        [System.Serializable]
        public class        TR : UnityEvent<Transform> { }
        [System.Serializable]
        public class        Bool : UnityEvent<bool> { }
        [System.Serializable]
        public class        Float : UnityEvent<float> { }
        [System.Serializable]
        public class        Int : UnityEvent<int> { }
        [System.Serializable]
        public class        String : UnityEvent<string> { }
        [System.Serializable]
        public class        ULong : UnityEvent<ulong> { }
        [System.Serializable]
        public class        Int2 : UnityEvent<int, int> { }
        [System.Serializable]
        public class        V2 : UnityEvent<UnityEngine.Vector2> { }
        [System.Serializable]
        public class        V2Int : UnityEvent<UnityEngine.Vector2Int> { }
        [System.Serializable]
        public class        V3 : UnityEvent<UnityEngine.Vector3> { }
        [System.Serializable]
        public class        V3Int : UnityEvent<UnityEngine.Vector3Int> { }
        [System.Serializable]
        public class        Color : UnityEvent<UnityEngine.Color> { }
        [System.Serializable]
        public class        Sprite : UnityEvent<UnityEngine.Sprite> { }
        [System.Serializable]
        public class        Entity : UnityEvent<IEntity> { }
        [System.Serializable]
        public class        EntityList : UnityEvent<IEntityList> { }
    }
}
