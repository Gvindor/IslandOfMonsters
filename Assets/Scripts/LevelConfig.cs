using UnityEngine;

namespace SF
{
    [CreateAssetMenu(menuName = "Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] bool boostEnabled;
        [SerializeField][Range(0, 1f)] float skinProgressFillWin = 0.333f;
        [SerializeField][Range(0, 1f)] float skinProgressFillLoose = 0.15f;
        [SerializeField] SceneReference scene;
        [SerializeField] int additionalRandomEnemies;
        [SerializeField] CharacterSkin[] enemies;

        public bool BoostEnabled => boostEnabled;
        public float SkinProgressFillWin => skinProgressFillWin;
        public float SkinProgressFillLoose => skinProgressFillLoose;
        public CharacterSkin[] Enemies => enemies;
        public SceneReference Scene => scene;
        public int AdditionalRandomEnemies => additionalRandomEnemies;
    }
}