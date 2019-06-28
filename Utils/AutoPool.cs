using UnityEngine;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace                           uStableObject
{
    public static class             AutoPool<T> where T : new()
    {
        static readonly object      ThreadSafe = new object();
        //static ConcurrentQueue<T>   Pool = new ConcurrentQueue<T>();
        static Queue<T>             Pool = new Queue<T>();

        public static T             Create()
        {
            T                       instance = default;

            lock (ThreadSafe)
            {
                if (Pool.Count > 0)
                {
                     instance = Pool.Dequeue();
                }
            }
            if (instance == null)
            {
                instance = new T();
            }
            return (instance);
        }

        public static void          DisposeList<L>(L obj/*, bool checkDoubles = false*/) where L : IList, T
        {
            if (obj != null)
            {
                obj.Clear();
                lock (ThreadSafe)
                {
                    /*if (checkDoubles)
                    {
                        if (Pool.Contains(obj))
                        {
                            Debug.LogError("Double object in pool: " + obj.GetType());
                        }
                    }*/
                    Pool.Enqueue(obj);
                }
            }
        }

        public static void          DisposeDict<L>(L obj) where L : IDictionary, T
        {
            if (obj != null)
            {
                obj.Clear();
                lock (ThreadSafe)
                {
                    Pool.Enqueue(obj);
                }
            }
        }

        public static void          Dispose(T obj)
        {
            if (obj != null)
            {
                lock (ThreadSafe)
                {
                    Pool.Enqueue(obj);
                }
            }
        }
    }

    /// <summary>
    /// For use with private constructor classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class             PrivatePool<T> where T : class
    {
        static readonly object      ThreadSafe = new object();
        static Queue<T>             Pool = new Queue<T>();

        public static T             Spawn()
        {
            T                       instance = default;

            lock (ThreadSafe)
            {
                if (Pool.Count > 0)
                {
                    instance = Pool.Dequeue();
                }
            }
            return (instance);
        }

        public static void          DisposeList<L>(L obj) where L : IList, T
        {
            if (obj != null)
            {
                obj.Clear();
                lock (ThreadSafe)
                {
                    Pool.Enqueue(obj);
                }
            }
        }

        public static void          DisposeDict<L>(L obj) where L : IDictionary, T
        {
            if (obj != null)
            {
                obj.Clear();
                lock (ThreadSafe)
                {
                    Pool.Enqueue(obj);
                }
            }
        }

        public static void          Dispose(T obj)
        {
            if (obj != null)
            {
                lock (ThreadSafe)
                {
                    Pool.Enqueue(obj);
                }
            }
        }
    }
}
