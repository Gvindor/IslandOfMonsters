using UnityEngine;
using TMPro;

namespace SF
{
    public class EndgameScreen : MonoBehaviour
    {
        [SerializeField] GameObject unlockDecorations;
        [SerializeField] TMP_Text levelLabel;

        private ProgressionManager pm;

        private void Awake()
        {
            pm = FindObjectOfType<ProgressionManager>();
        }

        private void OnEnable()
        {
            unlockDecorations.SetActive(pm.SkinUnlocked);

            levelLabel.text = $"LEVEL {pm.CurrentLevelIndex}";
        }
    }
}