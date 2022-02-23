using Cinemachine;
using UnityEngine;

namespace SF
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera gameplayCamera;
        [SerializeField] CinemachineVirtualCamera overviewCamera;
        [SerializeField] CinemachineVirtualCamera victoryCamera;

        private void Start()
        {
            UpdatePlayerReference();
        }

        public void UpdatePlayerReference()
        {
            var controllers = FindObjectsOfType<CombatController>();
            Transform player = null;

            foreach (var item in controllers)
            {
                if (item.CompareTag("Player"))
                {
                    player = item.transform;
                    break;
                }
            }

            if (player)
            {
                gameplayCamera.Follow = player;
                gameplayCamera.LookAt = player;

                victoryCamera.Follow = player;
                victoryCamera.LookAt = player;
            }
        }

        public void SwitchToGameplayCamera()
        {
            overviewCamera.gameObject.SetActive(false);
            gameplayCamera.gameObject.SetActive(true);
            victoryCamera.gameObject.SetActive(false);
        }

        public void SwitchToOverviewCamera()
        {
            gameplayCamera.gameObject.SetActive(false);
            overviewCamera.gameObject.SetActive(true);
            victoryCamera.gameObject.SetActive(false);
        }

        public void SwitchToVictoryCamera()
        {
            gameplayCamera.gameObject.SetActive(false);
            overviewCamera.gameObject.SetActive(false);
            victoryCamera.gameObject.SetActive(true);
        }
    }
}