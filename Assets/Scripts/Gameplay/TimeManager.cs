using UnityEngine;

namespace SF
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] float defaultBulletTimeDuration = 1f;
        [SerializeField][Range(0, 1f)] float slowTimeFactor = 0.3f;

        private float timer;

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.unscaledDeltaTime;

                if (timer <= 0)
                {
                    Time.timeScale = 1f;
                }
            }
        }

        public void BulletTime()
        {
            BulletTime(defaultBulletTimeDuration);
        }

        public void BulletTime(float duration)
        {
            if (timer < duration)
                timer = duration;

            Time.timeScale = slowTimeFactor;
        }
    }
}