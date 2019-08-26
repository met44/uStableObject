using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uStableObject.Data;

namespace                       uStableObject.Utilities
{
    public static class         CameraExtensions
    {
        public static Vector3   WorldToCanvasPoint(this Camera cam, Canvas canvas, Vector3 worldPos)
        {
            Vector3 onScreenPos = cam.WorldToViewportPoint(worldPos);
            onScreenPos.x = (onScreenPos.x - 0.5f) * (Screen.width / canvas.scaleFactor );
            onScreenPos.y = (onScreenPos.y - 0.5f) * (Screen.height / canvas.scaleFactor);
            onScreenPos.z = 0;
            return (onScreenPos);
        }

        public static Vector3   CanvasToWorldPoint(this Camera cam, Canvas canvas, Vector3 inCanvasWorldPos, float depth)
        {
            Vector3 localCanvasPos = (canvas.transform as RectTransform).InverseTransformPoint(inCanvasWorldPos);
            localCanvasPos.z = depth;
            return (cam.ScreenToWorldPoint(localCanvasPos));
        }

        public static Vector3   CanvasToCameraLocalPoint(this Camera cam, Canvas canvas, Vector3 inCanvasWorldPos, float depth)
        {
            Vector3             viewPortPos;

            if (Screen.width < Screen.height) //portrait
            {
                viewPortPos = new Vector3(inCanvasWorldPos.x / Screen.height, inCanvasWorldPos.y / Screen.width);
            }
            else //landscape
            {
                viewPortPos = new Vector3(inCanvasWorldPos.x / Screen.width, inCanvasWorldPos.y / Screen.height);
            }
            viewPortPos.z = depth;
            Vector3 cameraLocalPos = cam.transform.InverseTransformPoint(cam.ViewportToWorldPoint(viewPortPos));
            return (cameraLocalPos);
        }

        public static Vector3   CanvasToCameraWorldPoint(this Camera cam, Canvas canvas, Vector3 inCanvasWorldPos, float depth)
        {
            Vector3             viewPortPos;

            if (Screen.width < Screen.height) //portrait
            {
                viewPortPos = new Vector3(inCanvasWorldPos.x / Screen.height, inCanvasWorldPos.y / Screen.width);
            }
            else //landscape
            {
                viewPortPos = new Vector3(inCanvasWorldPos.x / Screen.width, inCanvasWorldPos.y / Screen.height);
            }
            viewPortPos.z = depth;
            Vector3 worldPos = cam.ViewportToWorldPoint(viewPortPos);
            return (worldPos);
        }

        public static Vector3   ScreenToCanvasPoint(this Camera cam, Canvas canvas, Vector3 screenPos)
        {
            Vector3 onScreenPos = cam.ScreenToViewportPoint(screenPos);
            onScreenPos.x = (onScreenPos.x - 0.5f) * (Screen.width / canvas.scaleFactor );
            onScreenPos.y = (onScreenPos.y - 0.5f) * (Screen.height / canvas.scaleFactor);
            return (onScreenPos);
        }

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

        public static Vector3   ViewportToPlane(this Camera cam, Vector3 viewportPos, Plane plane)
        {
            float               enter;
            Ray                 ray = cam.ViewportPointToRay(viewportPos);

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
