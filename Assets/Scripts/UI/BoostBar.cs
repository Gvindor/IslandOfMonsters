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

        private Animator animator;
        private FighterCharacterController character;

        private IEnumerator Start()
        {
            animator = GetComponent<Animator>();

            yield return null; //wait one frame for everything to initialize

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