using UnityEngine;

namespace SF
{
    public class Destructible : MonoBehaviour
    {
        [SerializeField] float minImpulse = 50;
        [SerializeField] Destructible[] chainTrigger;
        [SerializeField] GameObject[] deactivateOnTrigger;

        private Rigidbody rb;

        public bool IsDestroyed => !rb.isKinematic;

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
            rb.isKinematic = false;

            foreach (var item in chainTrigger)
            {
                item.Trigger();
            }

            foreach (var item in deactivateOnTrigger)
            {
                item.SetActive(false);
            }
        }
    }
}