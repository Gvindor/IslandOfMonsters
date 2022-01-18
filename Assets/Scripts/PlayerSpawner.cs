using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SF
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] bool spawnOnAwake = true;
        [SerializeField] int charactersToSpawn = 6;
        [SerializeField] float radius = 5;
        [SerializeField] GameObject prefabPlayer;
        [SerializeField] GameObject[] prefabsEnemy;

        private List<FighterCharacterController> aliveEnemies;
        private FighterCharacterController player;

        public int TotalCharacters => charactersToSpawn;
        public int AliveEnemiesCount => aliveEnemies != null ? aliveEnemies.Count : 0;

        public UnityEvent<FighterCharacterController> OnCharacterSpawned = new UnityEvent<FighterCharacterController>();
        public UnityEvent<FighterCharacterController> OnCharacterDead = new UnityEvent<FighterCharacterController>();

        private void Awake()
        {
            aliveEnemies = new List<FighterCharacterController>();

            SpawnPlayer();

            if (spawnOnAwake) SpawnEnemies();
        }

        public void SpawnPlayer()
        {
            var prefab = prefabPlayer;

            player = SpawnCharacter(0, prefab);

            OnCharacterSpawned.Invoke(player);
        }

        public void SpawnEnemies()
        {
            for (int i = 1; i < charactersToSpawn; i++)
            {
                var prefab = GetRandomEnemyPrefab();

                var enemy = SpawnCharacter(i, prefab);

                aliveEnemies.Add(enemy);

                OnCharacterSpawned.Invoke(enemy);
            }
        }

        private FighterCharacterController SpawnCharacter(int position, GameObject prefab)
        {
            var pos = GetCharacterPosition(position);
            var rot = GetCharacterRotation(position);

            var character = Instantiate(prefab, pos, rot);
            var controller = character.GetComponentInChildren<FighterCharacterController>();

            controller.OnDead.AddListener(() => { HandleCharacterDead(controller); });

            return controller;
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

        private void HandleCharacterDead(FighterCharacterController controller)
        {
            aliveEnemies.Remove(controller);
            OnCharacterDead.Invoke(controller);
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