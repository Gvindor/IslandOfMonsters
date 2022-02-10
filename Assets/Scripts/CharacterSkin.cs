using UnityEngine;

namespace SF
{
    [CreateAssetMenu(menuName = "Character Skin")]
    public class CharacterSkin : ScriptableObject
    {
        [SerializeField] GameObject playerPrefab;
        [SerializeField] GameObject enemyPrefab;

        [SerializeField] Sprite preview;

        public GameObject PlayerPrefab => playerPrefab;
        public GameObject EnemyPrefab => enemyPrefab;
        public Sprite Preview => preview;
    }
}