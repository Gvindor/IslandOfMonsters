using Cinemachine;
using UnityEngine;

namespace SF
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera gameplayCamera;
        [SerializeField] CinemachineVirtualCamera overviewCamera;

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
            }
        }

        public void SwitchToGameplayCamera()
        {
            overviewCamera.gameObject.SetActive(false);
            gameplayCamera.gameObject.SetActive(true);
        }

        public void SwitchToOverviewCamera()
        {
            gameplayCamera.gameObject.SetActive(false);
            overviewCamera.gameObject.SetActive(true);
        }
    }
}