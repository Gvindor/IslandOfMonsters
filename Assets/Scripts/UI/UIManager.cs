using System.Collections;
using UnityEngine;

namespace SF
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject gameplayUI;
        [SerializeField] GameObject winUI;
        [SerializeField] GameObject lostUI;
        [SerializeField] GameObject lobbyUI;

        public void SwitchToGameplay()
        {
            gameplayUI.SetActive(true);

            winUI.SetActive(false);
            lostUI.SetActive(false);
            lobbyUI.SetActive(false);
        }

        public void SwithToWinScreen()
        {
            winUI.SetActive(true);

            gameplayUI.SetActive(false);
            lobbyUI.SetActive(false);
            lostUI.SetActive(false);
        }

        public void SwithToLobby()
        {
            lobbyUI.SetActive(true);

            gameplayUI.SetActive(false);
            winUI.SetActive(false);
            lostUI.SetActive(false);
        }

        public void SwithToLostScreen()
        {
            lostUI.SetActive(true);

            lobbyUI.SetActive(false);
            gameplayUI.SetActive(false);
            winUI.SetActive(false);
        }
    }
}