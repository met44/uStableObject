using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uStableObject.Data;

namespace                       uStableObject
{
    public static class         CameraExtensions
    {
        /*
        public static Vector3   WorldToUGuiPoint(this Camera cam, Vector3 worldPos)
        {
            Vector3 onScreenPos = cam.WorldToViewportPoint(worldPos);
            onScreenPos.x = (onScreenPos.x - 0.5f) * (Screen.width / GameData.UI.HudCanvas.scaleFactor );
            onScreenPos.y = (onScreenPos.y - 0.5f) * (Screen.height / GameData.UI.HudCanvas.scaleFactor);
            onScreenPos.z = 0;
            return (onScreenPos);
        }

        public static Vector3   ScreenToUGuiPoint(this Camera cam, Vector3 screenPos)
        {
            Vector3 onScreenPos = cam.ScreenToViewportPoint(screenPos);
            onScreenPos.x = (onScreenPos.x - 0.5f) * (Screen.width / GameData.UI.HudCanvas.scaleFactor );
            onScreenPos.y = (onScreenPos.y - 0.5f) * (Screen.height / GameData.UI.HudCanvas.scaleFactor);
            return (onScreenPos);
        }
        */

        public static Vector3   ScreenToPlane(this Camera cam, Vector3 screenPos, Plane plane)
        {
            float               enter;
            Ray                 ray = cam.ScreenPointToRay(screenPos);

            if (plane.Raycast(ray, out enter))
            {
                return (ray.GetPoint(enter));
            }
            return (Vector3.zero);
        }

        /*
        public static Vector3   ViewportToWorldTerrain(this Camera cam, Vector3 vpPos)
        {
            float               enter;
            Ray                 ray = cam.ViewportPointToRay(vpPos);

            if (GameData.Math.WorldPlane.Raycast(ray, out enter))
            {
                return (ray.GetPoint(enter));
            }
            return (Vector3.zero);
        }
        */

#if UNITY_EDITOR
        [UnityEditor.MenuItem("CONTEXT/Camera/Log Culling Mask")]
        private static void     LogCullingMask()
        {
            if (UnityEditor.Selection.activeGameObject)
            {
                var cam = UnityEditor.Selection.activeGameObject.GetComponent<Camera>();
                if (cam)
                {
                    Debug.Log("Camera " + cam.name + " culling mask: " + cam.cullingMask);
                }
            }
        }
#endif
    }
}
