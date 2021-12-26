using UnityEngine;
using UnityEngine.AI;

namespace SF.AI
{
    public class RetreatAiState : AiState
    {
        [SerializeField] AiState nextState;
        [SerializeField] float minDistance = 5f;
        [SerializeField] float maxDistance = 8f;

        private FighterCharacterController controller;
        private NavMeshAgent agent;

        private Vector3 targetPosition;

        private void Awake()
        {
            controller = GetComponentInParent<FighterCharacterController>();
            agent = GetComponentInParent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            controller.ChangeTarget(null);

            Vector3 centerDir = -transform.position.normalized;
            Vector3 retreatDir = Quaternion.Euler(0, Random.Range(-30f, 30f), 0) * centerDir;
            float distance = Random.Range(minDistance, maxDistance);

            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position + retreatDir * distance, out hit, 3, NavMesh.AllAreas);
            targetPosition = hit.position;
        }

        private void Update()
        {
            if (Vector3.Distance(targetPosition, transform.position) > 0.2f)
            {
                if (agent.enabled)
                {
                    agent.SetDestination(targetPosition);
                    var input = agent.velocity;
                    controller.Move(new Vector2(input.x, input.z));
                }
            }
            else
                TransitionToState(nextState);
        }
    }
}