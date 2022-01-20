using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace SF
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] bool findPlayerOnStart = true;
        [SerializeField] Transform player;
        [SerializeField] CinemachineVirtualCamera cm_camera;


        private void Start()
        {
            var controllers = FindObjectsOfType<CombatController>();

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
                cm_camera.Follow = player;
                cm_camera.LookAt = player;
            }
        }
    }
}