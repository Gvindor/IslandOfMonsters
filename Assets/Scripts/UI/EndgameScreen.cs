using UnityEngine;
using TMPro;

namespace SF
{
    public class EndgameScreen : AUiScreen
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

        public override void SetVisible(bool visible)
        {
            if (visible) gameObject.SetActive(true);

            Animator.SetBool("Visible", visible);
        }
    }
}