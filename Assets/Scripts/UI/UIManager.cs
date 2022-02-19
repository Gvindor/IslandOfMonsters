using System.Collections;
using UnityEngine;

namespace SF
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] AUiScreen gameplayUI;
        [SerializeField] AUiScreen winUI;
        [SerializeField] AUiScreen lostUI;
        [SerializeField] AUiScreen lobbyUI;

        public void SwitchToGameplay()
        {
            gameplayUI.SetVisible(true);

            winUI.SetVisible(false);
            lostUI.SetVisible(false);
            lobbyUI.SetVisible(false);
        }

        public void SwithToWinScreen()
        {
            winUI.SetVisible(true);

            gameplayUI.SetVisible(false);
            lobbyUI.SetVisible(false);
            lostUI.SetVisible(false);
        }

        public void SwithToLobby()
        {
            lobbyUI.SetVisible(true);

            gameplayUI.SetVisible(false);
            winUI.SetVisible(false);
            lostUI.SetVisible(false);
        }

        public void SwithToLostScreen()
        {
            lostUI.SetVisible(true);

            lobbyUI.SetVisible(false);
            gameplayUI.SetVisible(false);
            winUI.SetVisible(false);
        }
    }
}