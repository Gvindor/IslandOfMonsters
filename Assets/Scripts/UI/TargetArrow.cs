using UnityEngine;
using UnityEngine.UI;

namespace SF
{
    public class TargetArrow : MonoBehaviour
    {
        private Image indicatorImage;

        public bool Active
        {
            get => transform.gameObject.activeInHierarchy;
            set => transform.gameObject.SetActive(value);
        }

        public Color Color
        {
            get => indicatorImage.color;
            set => indicatorImage.color = value;
        }

        void Awake()
        {
            indicatorImage = transform.GetComponent<Image>();
        }
    }
}