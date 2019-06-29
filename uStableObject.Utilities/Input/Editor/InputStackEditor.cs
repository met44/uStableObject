using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace                       uStableObject.Data
{
    [CustomEditor(typeof(InputStack))]
    public class                InputStackEditor : Editor
    {
        public override void    OnInspectorGUI()
        {
            base.OnInspectorGUI();

            InputStack inputStack = this.target as InputStack;
            serializedObject.Update();
            InputStack.Editor.ShowInspector(inputStack);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
