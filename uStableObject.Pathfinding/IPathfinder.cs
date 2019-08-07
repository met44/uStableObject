using System;
using System.Collections.Generic;

namespace uStableObject.Utilities
{
    public interface IPathfinder<T>
    {
        IReadOnlyCollection<T> GetPath(T from, T to);
        void Init(Action<T, List<T>> neighbours, Func<T, T, int> heuristic, int ancestorsFactor);
    }
}