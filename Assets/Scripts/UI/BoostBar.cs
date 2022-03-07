using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SF 
{ 
    public class BoostBar : MonoBehaviour
    {
        private const string VisibleKey = "Visible";

        [SerializeField] Image fill;
        [SerializeField] TMP_Text timeLabel;
        [SerializeField] GameManager gameManager;

        private Animator animator;
        private FighterCharacterController character;

        private void Start()
        {
            animator = GetComponent<Animator>();
            gameManager.OnGameStart.AddListener(OnGameStart);
        }

        private void OnGameStart()
        {
            var characters = FindObjectsOfType<FighterCharacterController>();

            foreach (var item in characters)
            {
                if (item.CompareTag("Player"))
                {
                    character = item;
                    break;
                }
            }
        }

        private void LateUpdate()
        {
            if (!character) return;

            SetBarVisible(character.ActiveBoost != null);

            if (character.ActiveBoost != null)
            {
                fill.fillAmount = character.BoostTimeLeft / character.ActiveBoost.Duration;

                timeLabel.text = $"{Mathf.CeilToInt(character.BoostTimeLeft)}s";
            }
        }

        private void SetBarVisible(bool visible)
        {
            animator.SetBool(VisibleKey, visible);
        }
    }
}