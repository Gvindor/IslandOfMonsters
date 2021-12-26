using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SF.AI
{
    public class FightAiState : AiState
    {
        private const float TargetCheckTime = 1f;

        [SerializeField] AiState retreatState;
        [SerializeField] AiState seekState;

        [SerializeField] [Min(0)] float minDistance = 0.6f;
        [SerializeField] [Min(0)] float lockDistance = 2f;
        [SerializeField] RangeFloat lockTime = new RangeFloat(10, 20);

        [SerializeField] int maxTargetsBeforeRetreat = 3;
        [SerializeField] [Min(0)] float retreatTargetRange = 1.5f;


        private List<CombatController> targets;

        private FighterCharacterController controller;
        private CombatController combatController;
        private NavMeshAgent agent;

        private float findTargetTimer;
        private float lockTimer;
        private CombatController activeTarget;

        private void Awake()
        {
            controller = GetComponentInParent<FighterCharacterController>();
            combatController = GetComponentInParent<CombatController>();
            agent = GetComponentInParent<NavMeshAgent>();

            targets = new List<CombatController>();
        }

        private void Start()
        {
            targets.AddRange(FindObjectsOfType<CombatController>());
            targets.Remove(combatController);
        }

        private void OnEnable()
        {
            combatController.OnHitRecived.AddListener(OnHitBy);

            lockTimer = lockTime.Random();
        }

        private void OnDisable()
        {

            combatController.OnHitRecived.RemoveListener(OnHitBy);
        }

        private void Update()
        {
            if (lockTimer <= 0)
                TransitionToState(seekState);
            else
                FindTarget();

            if (LockTargetIfInRange())
            {
                lockTimer -= Time.deltaTime;
            }

            if (activeTarget && Vector3.Distance(activeTarget.transform.position, transform.position) > minDistance)
            {
                if (agent.enabled)
                {
                    agent.SetDestination(activeTarget.transform.position);
                    var input = agent.velocity;
                    controller.Move(new Vector2(input.x, input.z));
                }
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

                float minDistance = float.MaxValue;
                int id = -1;

                int targetsInRange = 0;

                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].Character.IsDead || targets[i].Character.IsFainted) continue;

                    Vector3 dir = targets[i].transform.position - transform.position;
                    float distance = dir.magnitude;

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        id = i;
                    }
                    if (distance <= retreatTargetRange) targetsInRange++;
                }

                if (targetsInRange > maxTargetsBeforeRetreat)
                {
                    TransitionToState(retreatState);
                }
                else
                {
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
                }

                findTargetTimer = TargetCheckTime;
            }
        }

        private bool LockTargetIfInRange()
        {
            if (!activeTarget) return false;

            if (Vector3.Distance(transform.position, activeTarget.transform.position) < lockDistance)
            {
                controller.ChangeTarget(activeTarget.transform);
                return true;
            }
            else
            {
                controller.ChangeTarget(null);
                return false;
            }
        }

        private void OnHitBy(CombatController sender)
        {
            activeTarget = sender;
            findTargetTimer = TargetCheckTime;
        }
    }
}