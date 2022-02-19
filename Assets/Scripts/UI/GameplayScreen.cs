using System.Collections;
using UnityEngine;

namespace SF
{
    public class GameplayScreen : AUiScreen
    {
        [SerializeField] GameObject swipeToStart;

        public override void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);

            if (visible) swipeToStart.SetActive(true);
        }
    }
}