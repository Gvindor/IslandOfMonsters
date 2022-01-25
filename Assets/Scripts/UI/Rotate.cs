using UnityEngine;

namespace SF
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] bool useUnscaledTime = true;
        [SerializeField] float rotationSpeed = 60;

        private void Update()
        {
            float deltaAngle = useUnscaledTime ? rotationSpeed * Time.unscaledDeltaTime : rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, deltaAngle);
        }
    }
}