using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace                       uStableObject.EditorX
{
    [CustomEditor(typeof(GameEvent), true)]
    public class                GameEventEditor : Editor
    {
        public override void    OnInspectorGUI()
        {
            base.OnInspectorGUI();

            IGameEvent gEvent = this.target as IGameEvent;
            serializedObject.Update();
            gEvent.ShowInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(GameEventBase<>), true)]
    public class                GameEventBaseEditor : Editor
    {
        public override void    OnInspectorGUI()
        {
            base.OnInspectorGUI();

            IGameEvent gEvent = this.target as IGameEvent;
            serializedObject.Update();
            gEvent.ShowInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(GameEventBase<,>), true)]
    public class                GameEvent2BaseEditor : Editor
    {
        public override void    OnInspectorGUI()
        {
            base.OnInspectorGUI();

            IGameEvent gEvent = this.target as IGameEvent;
            serializedObject.Update();
            gEvent.ShowInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
