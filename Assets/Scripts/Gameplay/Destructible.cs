using UnityEngine;

namespace SF
{
    public class Destructible : MonoBehaviour
    {
        [SerializeField] float minImpulse = 50;
        [SerializeField] bool spawnFracturedObject = false;
        [SerializeField] GameObject fracturedPrefab;
        [SerializeField] Destructible[] chainTrigger;
        [SerializeField] GameObject[] deactivateOnTrigger;

        private Rigidbody rb;

        public bool IsDestroyed { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsDestroyed) return;

            if (collision.impulse.magnitude > minImpulse)
                Trigger();
        }

        public void Trigger()
        {
            IsDestroyed = true;

            foreach (var item in chainTrigger)
            {
                item.Trigger();
            }

            foreach (var item in deactivateOnTrigger)
            {
                item.SetActive(false);
            }

            if (spawnFracturedObject)
            {
                GameObject fractured = Instantiate(fracturedPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                rb.isKinematic = false;
            }
        }
    }
}