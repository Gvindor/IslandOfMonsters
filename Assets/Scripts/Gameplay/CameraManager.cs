using Cinemachine;
using UnityEngine;

namespace SF
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera gameplayCamera;

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
    }
}