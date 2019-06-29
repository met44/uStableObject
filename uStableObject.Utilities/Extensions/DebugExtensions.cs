using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    public static class         DebugEx
    {
        public static void      DrawRect(Vector3 worldMin, Vector3 worldMax, Color border, Color center, int duration = 1)
        {
            Debug.DrawLine(worldMin, worldMax, center, duration);
            Debug.DrawLine(worldMin, new Vector3(worldMax.x, worldMax.y, worldMin.z), border, duration);
            Debug.DrawLine(worldMin, new Vector3(worldMin.x, worldMax.y, worldMax.z), border, duration);
            Debug.DrawLine(worldMax, new Vector3(worldMax.x, worldMax.y, worldMin.z), border, duration);
            Debug.DrawLine(worldMax, new Vector3(worldMin.x, worldMax.y, worldMax.z), border, duration);
        }

        public static void      DrawArrow(Vector3 worldFrom, Vector3 worldTo, Vector3 normal, Color color, float width, int duration = 1)
        {
            Vector3 c = Vector3.Cross(worldTo - worldFrom, normal);
            Debug.DrawLine(worldFrom + c * width, worldFrom - c * width, color, duration);
            Debug.DrawLine(worldFrom + c * width, worldTo, color, duration);
            Debug.DrawLine(worldFrom - c * width, worldTo, color, duration);
        }
    }
}
