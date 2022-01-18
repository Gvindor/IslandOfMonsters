using System.Collections;
using UnityEngine;

namespace SF
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private RectTransform safeAreaRect;
        private Rect lastSafeArea;

        private void Awake()
        {
            safeAreaRect = GetComponent<RectTransform>();
            UpdateSizeToSafeArea();
        }

        private void Update()
        {
            if (Screen.safeArea != lastSafeArea)
            {
                lastSafeArea = Screen.safeArea;
                UpdateSizeToSafeArea();
            }
        }

        private void UpdateSizeToSafeArea()
        {
            var safeArea = Screen.safeArea;

            if (Screen.width > 0 && Screen.height > 0)
            {
                Vector2 anchorMin = safeArea.position;
                Vector2 anchorMax = safeArea.position + safeArea.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    safeAreaRect.anchorMin = anchorMin;
                    safeAreaRect.anchorMax = anchorMax;
                }
            }
        }
    }
}