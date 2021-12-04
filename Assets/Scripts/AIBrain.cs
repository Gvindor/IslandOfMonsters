using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class AIBrain : MonoBehaviour
    {
        [SerializeField] [Range(0, 5)] float speed = 0.5f;
        [SerializeField] [Min(0)] float minDistance = 0.6f;
        [SerializeField] [Min(0)] float lockDistance = 2f;

        private List<CombatController> targets;
        private FighterCharacterController controller;
        private CombatController combatController;
        private CameraManager cameraManager;

        private float findTargetTimer;
        private CombatController activeTarget;

        private void Start()
        {
            cameraManager = FindObjectOfType<CameraManager>();

            targets = new List<CombatController>();

            controller = GetComponent<FighterCharacterController>();
            combatController = GetComponent<CombatController>();

            targets.AddRange(FindObjectsOfType<CombatController>());
            targets.Remove(combatController);
        }

        private void Update()
        {
            FindTarget();
            LockTargetIfInRange();

            if (activeTarget && Vector3.Distance(activeTarget.transform.position, transform.position) > minDistance) 
            {
                var dir = (activeTarget.transform.position - transform.position).normalized;
                var input = dir * speed;
                controller.Move(new Vector2(input.x, input.z));
            }
            else
                controller.Move(Vector2.zero);
        }

        private void FindTarget()
        {
            if (findTargetTimer > 0)
            {
                findTargetTimer -= Time.deltaTime;
            }
            else
            {
                if (targets.Count == 0) return;

                float dist = float.MaxValue;
                int id = -1;

                for (int i = 0; i < targets.Count; i++)
                {
                    Vector3 dir = targets[i].transform.position - transform.position;
                    if (dir.magnitude < dist)
                    {
                        var tc = targets[i].GetComponent<FighterCharacterController>();
                        if (!tc.IsDead && !tc.IsFainted)
                        {
                            dist = dir.magnitude;
                            id = i;
                        }
                    }
                }

                bool isFocused = id != -1;
                if (isFocused)
                {
                    activeTarget = targets[id];
                }
                else
                {
                    activeTarget = null;
                    controller.ChangeTarget(null);
                }

                findTargetTimer = 0.5f;
            }
        }

        private void LockTargetIfInRange()
        {
            if (!activeTarget) return;

            if (Vector3.Distance(transform.position, activeTarget.transform.position) < lockDistance)
            {
                controller.ChangeTarget(activeTarget.transform);
            }
            else
            {
                controller.ChangeTarget(null);
            }
        }
    }
}