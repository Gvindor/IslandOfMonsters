using System.Collections;
using UnityEngine;

namespace SF
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject gameplayUI;
        [SerializeField] GameObject winUI;

        public void SwitchToGameplay()
        {
            gameplayUI.SetActive(true);

            winUI.SetActive(false);
        }

        public void SwithToWinScreen()
        {
            winUI.SetActive(true);

            gameplayUI.SetActive(false);
        }
    }
}