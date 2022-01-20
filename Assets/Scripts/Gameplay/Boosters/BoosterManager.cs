using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SF
{
    public class BoosterManager : MonoBehaviour
    {
        [SerializeField] Booster[] boosterPrefabs;

        [SerializeField][Min(0)] float width = 1f;
        [SerializeField][Min(0)] float height = 1f;

        [SerializeField] RangeFloat spawnTime = new RangeFloat(4, 6);

        [SerializeField] float spawnY = 10f;

        [SerializeField] int maxBoosterInstances = 5;
        [SerializeField] BoostData boostSettings;

        private float spawnTimer;
        private List<Booster> boosterInstances;

        private void Start()
        {
            boosterInstances = new List<Booster>();
            spawnTimer = spawnTime.Random();
        }

        public void SpawnBooster()
        {
            NavMeshHit hit;

            for (int i = 0; i < 50; i++)
            {
                var rnd_pos = GetRandomPosition();

                if (NavMesh.SamplePosition(rnd_pos, out hit, 3, NavMesh.AllAreas))
                {
                    SpawnBooster(hit.position);
                    break;
                }
            }
            
        }

        private void SpawnBooster(Vector3 position)
        {
            position.y = spawnY;

            var prefab = boosterPrefabs[Random.Range(0, boosterPrefabs.Length)];
            var booster = Instantiate(prefab, position, Quaternion.identity);

            boosterInstances.Add(booster);
            booster.OnPicked.AddListener(OnBoosterPicked);
        }

        private Vector3 GetRandomPosition()
        {
            float x_min = transform.position.x - width * 0.5f;
            float x_max = transform.position.x + width * 0.5f;

            float z_min = transform.position.z - height * 0.5f;
            float z_max = transform.position.z + height * 0.5f;

            float x = Random.Range(x_min, x_max);
            float y = transform.position.y;
            float z = Random.Range(z_min, z_max);

            return new Vector3(x, y, z);            
        }

        public void ApplyBoostToCharacter(FighterCharacterController character)
        {
            character.ApplyBoost(boostSettings);
        }

        private void Update()
        {
            if (spawnTimer > 0)
                spawnTimer -= Time.deltaTime;
            else
            {
                if (boosterInstances.Count < maxBoosterInstances)
                    SpawnBooster();
                spawnTimer = spawnTime.Random(); //Reset the timer anyway. We don't want to spawn a new booster immediately after another got picked.
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);

            Gizmos.DrawCube(transform.position, new Vector3(width, 0.1f, height));
        }

        private void OnBoosterPicked(Booster booster)
        {
            boosterInstances.Remove(booster);
        }
    }
}