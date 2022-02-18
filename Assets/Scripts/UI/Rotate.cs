using UnityEngine;

namespace SF
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] bool useUnscaledTime = true;
        [SerializeField] Vector3 rotationSpeed;

        private void Update()
        {
            Vector3 deltaAngle = useUnscaledTime ? rotationSpeed * Time.unscaledDeltaTime : rotationSpeed * Time.deltaTime;

            transform.Rotate(deltaAngle);
        }
    }
}