using UnityEngine;

namespace SF
{
    public abstract class AUiScreen : MonoBehaviour
    {
        private Animator animator;
        protected Animator Animator
        {
            get
            {
                if (!animator)
                    animator = GetComponent<Animator>();

                return animator;
            }
        }

        public abstract void SetVisible(bool visible);

        private void OnHideAnimationFinished()
        {
            gameObject.SetActive(false);
        }
    }
}