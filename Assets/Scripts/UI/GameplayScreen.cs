using System.Collections;
using UnityEngine;

namespace SF
{
    public class GameplayScreen : AUiScreen
    {
        public override void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}