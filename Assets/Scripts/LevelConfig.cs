using UnityEngine;

namespace SF
{
    [CreateAssetMenu(menuName = "Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] bool boostEnabled;
        [SerializeField][Range(0, 1f)] float skinProgressFill = 0.5f;
        [SerializeField] SceneReference scene;
        [SerializeField] int additionalRandomEnemies;
        [SerializeField] CharacterSkin[] enemies;

        public bool BoostEnabled => boostEnabled;
        public float SkinProgressFill => skinProgressFill;
        public CharacterSkin[] Enemies => enemies;
        public SceneReference Scene => scene;
        public int AdditionalRandomEnemies => additionalRandomEnemies;
    }
}