using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace                               uStableObject.Utilities
{
    public static class                 EventSystemExtentions
    {
        static BaseInputModule          registeredInputModule;
        static System.Func<GameObject>  GetCurrentFocusedGameObject;

        [RuntimeInitializeOnLoadMethod]
        static void                     ResetDelegate() //ensures it works with domain reload off
        {
            registeredInputModule = null;
            GetCurrentFocusedGameObject = null;
        }

        static void EnsureDelegate(EventSystem evSys)
        {
            if (registeredInputModule != evSys.currentInputModule)
            {
                var method = (evSys.currentInputModule as StandaloneInputModule).GetType().GetMethod("GetCurrentFocusedGameObject", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                GetCurrentFocusedGameObject = (System.Func<GameObject>)System.Delegate.CreateDelegate(typeof(System.Func<GameObject>), evSys.currentInputModule, method);
                registeredInputModule = evSys.currentInputModule;
            }
        }

        public static bool              WorkingIsPointerOverGameObject(this EventSystem evSys)
        {
            EnsureDelegate(evSys);
            var go = GetCurrentFocusedGameObject();
            return (go);
        }

        public static GameObject        WorkingGetCurrentSelectedGameObject(this EventSystem evSys)
        {
            EnsureDelegate(evSys);
            var go = GetCurrentFocusedGameObject();
            return (go);
        }
    }
}
