using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;

namespace SF
{
    public class SwipeToStart : MonoBehaviour
    {
        private bool swiped;

        public UnityEvent OnSwipe = new UnityEvent();

        private void Update()
        {
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
            {
                if (!swiped)
                {
                    swiped = true;
                    OnSwipe.Invoke();
                }
            }
            else
            {
                swiped = false;
            }
        }
    }
}