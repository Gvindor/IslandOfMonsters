using System.Collections;
using UnityEngine;

namespace SF
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] bool spawnOnAwake = true;
        [SerializeField] int charactersToSpawn = 6;
        [SerializeField] float radius = 5;
        [SerializeField] GameObject prefabPlayer;
        [SerializeField] GameObject[] prefabsEnemy;

        private void Awake()
        {
            if (spawnOnAwake) Spawn();
        }

        public void Spawn()
        {
            for (int i = 0; i < charactersToSpawn; i++)
            {
                var prefab = i == 0 ? prefabPlayer : GetRandomEnemyPrefab();

                var pos = GetCharacterPosition(i);
                var rot = GetCharacterRotation(i);

                Instantiate(prefab, pos, rot);
            }
        }

        private GameObject GetRandomEnemyPrefab()
        {
            return prefabsEnemy[Random.Range(0, prefabsEnemy.Length)];
        }

        private Vector3 GetCharacterPosition(int index)
        {
            float angle = 360f / charactersToSpawn * index;
            var position = new Vector3(0, 0, radius);

            position = Quaternion.Euler(0, angle, 0) * position;
            position = transform.TransformPoint(position);

            return position;
        }

        private Quaternion GetCharacterRotation(int index)
        {
            var pos = GetCharacterPosition(index);
            var forward = Vector3.Normalize(pos - transform.position);
            var rotation = Quaternion.LookRotation(forward, Vector3.up);

            return rotation;
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < charactersToSpawn; i++)
            {
                Gizmos.color = i == 0 ? Color.yellow : Color.blue;

                var pos = GetCharacterPosition(i);
                Gizmos.DrawSphere(pos, 0.2f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}