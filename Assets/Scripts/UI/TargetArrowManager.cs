using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class TargetArrowManager : MonoBehaviour
    {
        [Range(0.5f, 0.9f)]
        [SerializeField] private float screenBoundOffset = 0.9f;
        [SerializeField] TargetArrow arrowPrefab;

        private Camera mainCamera;
        private Vector3 screenCentre;
        private Vector3 screenBounds;

        private ObjectPool<TargetArrow> arrowsPool;
        private List<Target> targets = new List<Target>();

        public static Action<Target, bool> TargetStateChanged;

        void Awake()
        {
            mainCamera = Camera.main;
            screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
            screenBounds = screenCentre * screenBoundOffset;
            TargetStateChanged += HandleTargetStateChanged;

            arrowsPool = new ObjectPool<TargetArrow>(transform, arrowPrefab);
        }

        void LateUpdate()
        {
            DrawIndicators();
        }

        void DrawIndicators()
        {
            foreach (Target target in targets)
            {
                Vector3 screenPosition = GetScreenPosition(mainCamera, target.transform.position);
                bool isTargetVisible = IsTargetVisible(screenPosition);
                TargetArrow indicator = null;

                if (!isTargetVisible)
                {
                    float angle = float.MinValue;
                    GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds);
                    indicator = GetIndicator(ref target.indicator); // Gets the arrow indicator from the pool.
                    indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); // Sets the rotation for the arrow indicator.
                }
                else
                {
                    if (target.indicator)
                        target.indicator.Active = false;
                    target.indicator = null;
                }
                if (indicator)
                {
                    indicator.Color = target.TargetColor;// Sets the image color of the indicator.
                    indicator.transform.position = screenPosition; //Sets the position of the indicator on the screen.
                }
            }
        }

        private void HandleTargetStateChanged(Target target, bool active)
        {
            if (active)
            {
                targets.Add(target);
            }
            else
            {
                if (target.indicator)
                    target.indicator.Active = false;
                target.indicator = null;
                targets.Remove(target);
            }
        }

        private TargetArrow GetIndicator(ref TargetArrow indicator)
        {
            if (indicator == null)
            {
                indicator = arrowsPool.GetPooledObject();
                indicator.Active = true;
            }

            return indicator;
        }

        private void OnDestroy()
        {
            TargetStateChanged -= HandleTargetStateChanged;
        }

        public static Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            return screenPosition;
        }

        private bool IsTargetVisible(Vector3 screenPosition)
        {
            bool isTargetVisible = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;
            return isTargetVisible;
        }

        private void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds)
        {
            // Our screenPosition's origin is screen's bottom-left corner.
            // But we have to get the arrow's screenPosition and rotation with respect to screenCentre.
            screenPosition -= screenCentre;

            // When the targets are behind the camera their projections on the screen (WorldToScreenPoint) are inverted,
            // so just invert them.
            if (screenPosition.z < 0)
            {
                screenPosition *= -1;
            }

            // Angle between the x-axis (bottom of screen) and a vector starting at zero(bottom-left corner of screen) and terminating at screenPosition.
            angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
            // Slope of the line starting from zero and terminating at screenPosition.
            float slope = Mathf.Tan(angle);

            // Two point's line's form is (y2 - y1) = m (x2 - x1) + c, 
            // starting point (x1, y1) is screen botton-left (0, 0),
            // ending point (x2, y2) is one of the screenBounds,
            // m is the slope
            // c is y intercept which will be 0, as line is passing through origin.
            // Final equation will be y = mx.
            if (screenPosition.x > 0)
            {
                // Keep the x screen position to the maximum x bounds and
                // find the y screen position using y = mx.
                screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
            }
            else
            {
                screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
            }
            // Incase the y ScreenPosition exceeds the y screenBounds 
            if (screenPosition.y > screenBounds.y)
            {
                // Keep the y screen position to the maximum y bounds and
                // find the x screen position using x = y/m.
                screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
            }
            else if (screenPosition.y < -screenBounds.y)
            {
                screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
            }
            // Bring the ScreenPosition back to its original reference.
            screenPosition += screenCentre;
        }
    }
}