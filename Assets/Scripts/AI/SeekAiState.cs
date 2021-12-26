using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SF.AI
{
    public class SeekAiState : AiState
    {
        [SerializeField] AiState nextState;
        [SerializeField] float distance = 2f;

        private FighterCharacterController controller;
        private CombatController combatController;
        private NavMeshAgent agent;

        private CombatController activeTarget;
        private List<CombatController> targets;

        private void Awake()
        {
            controller = GetComponentInParent<FighterCharacterController>();
            combatController = GetComponentInParent<CombatController>();
            agent = GetComponentInParent<NavMeshAgent>();

            targets = new List<CombatController>();
        }

        private void OnEnable()
        {
            if (targets.Count == 0)
            {
                FindAllTargets();
            }

            FindRandomTarget();
            controller.ChangeTarget(null);
        }

        private void Update()
        {
            if (!activeTarget)
            {
                TransitionToState(nextState);
                return;
            }

            if (activeTarget.Character.IsDead)
            {
                FindRandomTarget();
                return;
            }

            if (Vector3.Distance(activeTarget.transform.position, transform.position) > distance)
            {
                if (agent.enabled)
                {
                    agent.SetDestination(activeTarget.transform.position);
                    var input = agent.velocity;
                    controller.Move(new Vector2(input.x, input.z));

                    Debug.DrawLine(transform.position, activeTarget.transform.position, Color.cyan);
                }
            }
            else
                TransitionToState(nextState);
        }

        private void FindAllTargets()
        {
            targets.Clear();
            targets.AddRange(FindObjectsOfType<CombatController>());
            targets.Remove(combatController);
        }

        private void FindRandomTarget()
        {
            var validTargets = targets.FindAll(t => !t.Character.IsDead);

            if (validTargets.Count > 0)
                activeTarget = validTargets[Random.Range(0, validTargets.Count)];
            else
                activeTarget = null;
        }
    }
}