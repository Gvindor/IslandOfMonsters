using System.Collections;
using UnityEngine;

namespace SF
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private Color targetColor = Color.red;

        [HideInInspector] public TargetArrow indicator;

        public Color TargetColor => targetColor;

        private void OnEnable()
        {
            TargetArrowManager.TargetStateChanged?.Invoke(this, true);
        }

        private void OnDisable()
        {
            TargetArrowManager.TargetStateChanged?.Invoke(this, false);
        }

        public float GetDistanceFromCamera(Vector3 cameraPosition)
        {
            float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
            return distanceFromCamera;
        }
    }
}