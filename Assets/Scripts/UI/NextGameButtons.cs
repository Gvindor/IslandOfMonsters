using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SF
{
    public class NextGameButtons : MonoBehaviour
    {
        [SerializeField] Button nextGameButton;
        [SerializeField] Button applySkinButton;

        private ProgressionManager pm;

        public UnityEvent OnNextGame = new UnityEvent();

        private void Awake()
        {
            pm = FindObjectOfType<ProgressionManager>();

            nextGameButton.onClick.AddListener(OnNextGameClick);
            applySkinButton.onClick.AddListener(OnApplySkinClick);
        }

        private void OnEnable()
        {
            nextGameButton.gameObject.SetActive(!pm.SkinUnlocked);
            applySkinButton.gameObject.SetActive(pm.SkinUnlocked);
        }

        private void OnNextGameClick()
        {
            OnNextGame.Invoke();
        }

        private void OnApplySkinClick()
        {
            if (pm.SkinUnlocked)
            {
                pm.UseNextSkin();
            }

            OnNextGame.Invoke();
        }
    }
}