using System;
using System.Collections.Generic;

namespace uStableObject.Utilities
{
    public interface IPathfinder<T>
    {
        IEnumerable<T> GetPath(T from, T to);
        void Init(Func<T, IEnumerable<T>> neighbours, Func<T, T, int> heuristic);
    }
}