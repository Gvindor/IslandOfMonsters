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
        [SerializeField] bool useSkins = true;
        [SerializeField] bool circleMode = true;
        [SerializeField] Transform[] spawnPoints;

        [Header("Prefabs")] //Only used when ProgressionManager isn't present.
        [SerializeField] GameObject prefabPlayer;
        [SerializeField] GameObject prefabEnemy;

        private List<FighterCharacterController> aliveEnemies;
        private FighterCharacterController player;

        public int TotalCharacters => charactersToSpawn;
        public int AliveEnemiesCount => aliveEnemies != null ? aliveEnemies.Count : 0;

        public UnityEvent<FighterCharacterController> OnCharacterSpawned = new UnityEvent<FighterCharacterController>();
        public UnityEvent<FighterCharacterController> OnCharacterDead = new UnityEvent<FighterCharacterController>();

        private void Awake()
        {
            aliveEnemies = new List<FighterCharacterController>();

            if (spawnOnAwake)
            {
                SpawnPlayer();
                SpawnEnemies();
            }
        }

        public void SpawnPlayer()
        {
            var prefab = prefabPlayer;

            if (useSkins)
            {
                var pm = FindObjectOfType<ProgressionManager>();

                if (pm)
                    prefab = pm.ActiveSkin.PlayerPrefab;
                else
                    Debug.LogError("Can't find Progression Manager.", this);
            }

            player = SpawnCharacter(0, prefab);

            OnCharacterSpawned.Invoke(player);
        }

        public void SpawnEnemies()
        {
            var pm = FindObjectOfType<ProgressionManager>();

            var prefabsPool = new List<GameObject>();
            if (pm)
            {
                var skins = pm.GetUnusedSkins();
                foreach (var item in skins)
                {
                    prefabsPool.Add(item.EnemyPrefab);
                }
            }

            for (int i = 1; i < charactersToSpawn; i++)
            {
                var prefab = prefabEnemy;
                if (prefabsPool.Count > 0)
                {
                    prefab = prefabsPool[0];
                    prefabsPool.RemoveAt(0);
                }

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

        private Vector3 GetCharacterPosition(int index)
        {
            Vector3 position = transform.position;

            if (circleMode) 
            { 
                float angle = 360f / charactersToSpawn * index;
                position = new Vector3(0, 0, radius);

                position = Quaternion.Euler(0, angle, 0) * position;
                position = transform.TransformPoint(position);
            }
            else
            {
                if (spawnPoints.Length > 0)
                {
                    index = index % spawnPoints.Length;
                    position = spawnPoints[index].position;
                }
            }

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
            if (circleMode)
            {
                Gizmos.DrawWireSphere(transform.position, radius);
            }
            else
            {
                if (spawnPoints.Length > 1) 
                {
                    if (spawnPoints.Length > 2)
                    {
                        for (int i = 0; i < spawnPoints.Length - 1; i++)
                        {
                            Gizmos.DrawLine(spawnPoints[i].position, spawnPoints[i + 1].position);
                        }
                    }

                    Gizmos.DrawLine(spawnPoints[0].position, spawnPoints[spawnPoints.Length - 1].position);
                }
            }
        }
    }
}