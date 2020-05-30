using UnityEngine;

namespace uStableObject.Data
{
    public interface IDisplayable
    {
        Sprite Icon { get; }
        string Name { get; }
    }
}