using UnityEngine;

namespace SF
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private Color targetColor = Color.red;

        [HideInInspector] public TargetArrow indicator;

        public Color TargetColor => targetColor;

        private TargetArrowManager manager;

        private void OnEnable()
        {
            manager = FindObjectOfType<TargetArrowManager>(true);

            manager?.RegisterTarget(this);
        }

        private void OnDisable()
        {
            manager?.UnregisterTarget(this);
        }

        public float GetDistanceFromCamera(Vector3 cameraPosition)
        {
            float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
            return distanceFromCamera;
        }
    }
}