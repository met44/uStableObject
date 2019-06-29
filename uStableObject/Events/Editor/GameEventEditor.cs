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

            GameEvent gEvent = this.target as GameEvent;
            serializedObject.Update();
            GameEvent.Editor.ShowInspector(gEvent);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
