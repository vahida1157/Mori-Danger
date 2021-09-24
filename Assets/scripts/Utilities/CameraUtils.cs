using System;
using System.Security;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class CameraUtils : MonoBehaviour
    {
        private static Ray _cameraRay;
        private static Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            _cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
        }

        public static void PlayerLookAtMouseCursor(Transform transform)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(_cameraRay, out var rayLength))
            {
                var pointToLock = _cameraRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLock.x, transform.position.y, pointToLock.z));
            }
        }

        public static void LookAtMouseCursor(Transform transform, Vector3 planePoint)
        {
            var groundPlane = new Plane(Vector3.up, planePoint);

            if (groundPlane.Raycast(_cameraRay, out var rayLength))
            {
                var pointToLock = _cameraRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLock.x, transform.position.y, pointToLock.z));
            }
        }

        public static Vector3 getInGameMousePosition(Plane plane)
        {
            return plane.Raycast(_cameraRay, out var rayLength) ? _cameraRay.GetPoint(rayLength) : Vector3.zero;
        }
    }
}