using System.Collections;
using UnityEngine;

namespace SF
{
    public class LobbyScreen : AUiScreen
    {
        public override void SetVisible(bool visible)
        {
            if (visible) gameObject.SetActive(true);

            Animator.SetBool("Visible", visible);
        }
    }
}