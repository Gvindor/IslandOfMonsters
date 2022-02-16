using UnityEngine;

namespace SF
{
    public class Destructible : MonoBehaviour
    {
        private const float DespawnTime = 5f;
        private const float DespawnChance = 0.6f;

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
            if (IsDestroyed) return;

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

                var rbs = fractured.GetComponentsInChildren<Rigidbody>();

                foreach (var rb in rbs)
                {
                    Despawn(rb.gameObject);
                }

                Destroy(gameObject);
            }
            else
            {
                rb.isKinematic = false;
                Despawn(gameObject);
            }
        }

        private void Despawn(GameObject gameObject)
        {
            float dice = Random.Range(0, 1f);

            if (dice <= DespawnChance)
            {
                var shrinker = gameObject.AddComponent<ShrinkAndDestroy>();
                float time = Random.Range(DespawnTime * 0.9f, DespawnTime * 1.1f);
                shrinker.Shrink(time);
            }
        }
    }
}