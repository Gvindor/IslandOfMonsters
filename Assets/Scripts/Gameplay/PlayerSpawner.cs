using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SF
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] bool spawnOnAwake = true;
        [SerializeField] Transform[] spawnPoints;

        private List<FighterCharacterController> aliveEnemies;
        private FighterCharacterController player;
        private ProgressionManager pm;

        public int TotalCharacters { get; private set; }
        public int AliveEnemiesCount => aliveEnemies != null ? aliveEnemies.Count : 0;

        public UnityEvent<FighterCharacterController> OnCharacterSpawned = new UnityEvent<FighterCharacterController>();
        public UnityEvent<FighterCharacterController> OnCharacterDead = new UnityEvent<FighterCharacterController>();

        private void Awake()
        {
            aliveEnemies = new List<FighterCharacterController>();

            pm = FindObjectOfType<ProgressionManager>();

            if (spawnOnAwake)
            {
                SpawnPlayer();
                SpawnEnemies();
            }
        }

        public void SpawnPlayer()
        {
            var prefab = pm.ActiveSkin.PlayerPrefab;

            player = SpawnCharacter(0, prefab);

            TotalCharacters++;

            OnCharacterSpawned.Invoke(player);
        }

        public void SpawnEnemies()
        {
            var skins = pm.GetEnemiesForLevel();

            for (int i = 0; i < skins.Length; i++)
            {
                var enemy = SpawnCharacter(i + 1, skins[i].EnemyPrefab);

                aliveEnemies.Add(enemy);

                TotalCharacters++;

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

            if (spawnPoints.Length > 0)
            {
                index = index % spawnPoints.Length;
                position = spawnPoints[index].position;
            }

            return position;
        }

        private Quaternion GetCharacterRotation(int index)
        {
            var pos = GetCharacterPosition(index);
            var forward = Vector3.Normalize(transform.position - pos);
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
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Gizmos.color = i == 0 ? Color.yellow : Color.blue;

                var pos = GetCharacterPosition(i);
                Gizmos.DrawSphere(pos, 0.2f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

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