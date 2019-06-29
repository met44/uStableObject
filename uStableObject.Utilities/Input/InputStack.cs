using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using uStableObject.Utilities;

namespace                       uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Input/Input Stack", order = 3)]
    public class                InputStack : ScriptableObject
    {
        #region Members
        Dictionary<InputAction, List<System.Action>> _actionStacks = new Dictionary<InputAction, List<System.Action>>();
        #endregion

        #region Properties
        public static bool      OverUI { get; private set; }
        #endregion

        #region Triggers
        public void             ProcessInput()
        {
            var selectedGo = EventSystem.current.WorkingGetCurrentSelectedGameObject();
            OverUI = selectedGo ? selectedGo.transform as RectTransform : false;
            foreach (var actionKvp in this._actionStacks)
            {
                try
                {
                    if (actionKvp.Value.Count > 0)
                    {
                        if (actionKvp.Key.Check())
                        {
                            actionKvp.Value[0]();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        public void             TriggerAction(InputAction action)
        {
            List<System.Action> stack;

            if (this._actionStacks.TryGetValue(action, out stack) && stack.Count > 0)
            {
                stack[0]();
            }
        }

        public void             RegisterActionCallback(InputAction action, System.Action callback)
        {
            List<System.Action> stack;

            if (!this._actionStacks.TryGetValue(action, out stack))
            {
                stack = new List<System.Action>();
                this._actionStacks.Add(action, stack);
            }
            stack.Insert(0, callback);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void             UnregisterActionCallback(InputAction action, System.Action callback)
        {
            List<System.Action> stack;

            if (this._actionStacks.TryGetValue(action, out stack))
            {
                if (!stack.Remove(callback))
                {
                    Debug.LogError("Input Stack Error. Unregistering wrong callback");
                }
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public void             TryUnregisterActionCallback(InputAction action, System.Action callback)
        {
            List<System.Action> stack;

            if (this._actionStacks.TryGetValue(action, out stack))
            {
                stack.Remove(callback);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }
        #endregion

#if UNITY_EDITOR
        //this works around internal being impossible to properly make an editor for
        public static class     Editor
        {
            public static void  ShowInspector(InputStack stack)
            {
                if (stack._actionStacks != null)
                {
                    UnityEditor.EditorGUILayout.Separator();
                    UnityEditor.EditorGUILayout.LabelField("Action Stacks");
                    foreach (var kvp in stack._actionStacks)
                    {
                        int     c = 0;

                        UnityEditor.EditorGUILayout.ObjectField(kvp.Key, kvp.Key.GetType(), false);
                        foreach (var callback in kvp.Value)
                        {
                            if (callback.Target is Object)
                            {
                                Object obj = callback.Target as Object;
                                UnityEditor.EditorGUILayout.ObjectField("[" + c + "] " + callback.Method.Name, obj, obj.GetType(), false);
                            }
                            else
                            {
                                UnityEditor.EditorGUILayout.LabelField("[" + c + "] " + callback.Method.Name);
                            }
                            ++c;
                        }
                        UnityEditor.EditorGUILayout.Space();
                    }
                }
            }
        }
#endif
    }
}
