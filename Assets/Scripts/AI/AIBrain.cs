using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SF.AI;

namespace SF
{
    public class AIBrain : MonoBehaviour
    {
        [SerializeField] AiState defaultState;

        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;

            defaultState.gameObject.SetActive(true);
        }

        private void LateUpdate()
        {
            agent.nextPosition = transform.position;
        }
    }
}