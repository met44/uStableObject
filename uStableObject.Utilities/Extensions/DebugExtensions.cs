using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    public static class         DebugEx
    {
        public static void      DrawCube(Vector3 position, float size, Color color)
        {
#if UNITY_EDITOR
            var prevColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.CubeHandleCap(0, position, Quaternion.identity, size, EventType.Repaint);
            UnityEditor.Handles.color = prevColor;
#endif
        }

        public static void      DrawCube(Vector3 position, Quaternion rotation, float size, Color color)
        {
#if UNITY_EDITOR
            var prevColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.CubeHandleCap(0, position, rotation, size, EventType.Repaint);
            UnityEditor.Handles.color = prevColor;
#endif
        }

        public static void      DrawRect(Vector3 worldMin, Vector3 worldMax, Color border, Color center, int duration = 1, bool depthTest = true)
        {
            Debug.DrawLine(worldMin, worldMax, center, duration);
            Debug.DrawLine(worldMin, new Vector3(worldMax.x, worldMax.y, worldMin.z), border, duration, depthTest);
            Debug.DrawLine(worldMin, new Vector3(worldMin.x, worldMax.y, worldMax.z), border, duration, depthTest);
            Debug.DrawLine(worldMax, new Vector3(worldMax.x, worldMax.y, worldMin.z), border, duration, depthTest);
            Debug.DrawLine(worldMax, new Vector3(worldMin.x, worldMax.y, worldMax.z), border, duration, depthTest);
        }

        public static void      DrawArrow(Vector3 worldFrom, Vector3 worldTo, Vector3 normal, Color color, float width, int duration = 1, bool depthTest = true)
        {
            Vector3 c = Vector3.Cross(worldTo - worldFrom, normal);
            Debug.DrawLine(worldFrom + c * width, worldFrom - c * width, color, duration, depthTest);
            Debug.DrawLine(worldFrom + c * width, worldTo, color, duration, depthTest);
            Debug.DrawLine(worldFrom - c * width, worldTo, color, duration, depthTest);
        }
    }
}
