using UnityEngine;

namespace SF
{
    public class ShrinkAndDestroy : MonoBehaviour
    {
        private const float ShrinkTime = 0.25f;

        private Vector3 startScale;
        private float shrinkTimer;
        private float delayTimer;
        private bool started;

        public void Shrink(float delay)
        {
            started = true;
            shrinkTimer = ShrinkTime;
            delayTimer = delay;

            startScale = transform.localScale;
        }

        private void Update()
        {
            if (started)
            {
                if (delayTimer > 0)
                {
                    delayTimer -= Time.deltaTime;
                }
                else
                {
                    shrinkTimer -= Time.deltaTime;

                    if (shrinkTimer > 0)
                    {
                        float t = shrinkTimer / ShrinkTime;

                        transform.localScale = Vector3.Lerp(Vector3.zero, startScale, t);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}