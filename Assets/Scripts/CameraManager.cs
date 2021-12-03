using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace SF
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] bool findPlayerOnStart = true;
        [SerializeField] Transform player;
        [SerializeField] CinemachineTargetGroup targetGroup;

        private List<CombatController> targets = new List<CombatController>();

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
                targetGroup.AddMember(player, 1, 1);
        }

        private void LateUpdate()
        {
            for (int i = targets.Count - 1; i >= 0; i--)
            {
                if (targets[i].Character.IsDead)
                    RemoveTarget(targets[i]);
            }
        }

        public void AddTarget(CombatController controller)
        {
            if (!controller) return;

            if (!targets.Contains(controller))
            {
                targets.Add(controller);
                targetGroup.AddMember(controller.transform, 1, 1);
            }
        }

        public void RemoveTarget(CombatController controller)
        {
            if (!controller) return;

            targets.Remove(controller);
            targetGroup.RemoveMember(controller.transform);
        }
    }
}