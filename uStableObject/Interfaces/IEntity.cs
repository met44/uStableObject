using UnityEngine;

namespace uStableObject.Data
{
    public interface IEntity
    {
        Sprite Icon { get; }
        string Name { get; }
    }
}