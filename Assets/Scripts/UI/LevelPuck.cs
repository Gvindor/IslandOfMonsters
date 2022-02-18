using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SF
{
    public class LevelPuck : MonoBehaviour
    {
        [SerializeField] TMP_Text label;
        [SerializeField] Sprite passedSprite;
        [SerializeField] Sprite activeSprite;
        [SerializeField] Sprite lockedSprite;

        public string Text
        {
            get => label.text;
            set => label.text = value;
        }

        public StateEnum State
        {
            set
            {
                var renderer = GetComponent<Image>();

                switch (value)
                {
                    case StateEnum.Passed:
                        renderer.sprite = passedSprite;
                        break;
                    case StateEnum.Active:
                        renderer.sprite = activeSprite;
                        break;
                    case StateEnum.Locked:
                        renderer.sprite = lockedSprite;
                        break;
                }
            }
        }

        public enum StateEnum
        {
            Passed,
            Active,
            Locked
        }
    }
}