using System.Collections;
using UnityEngine;

namespace SF
{
    public class AIBrain : MonoBehaviour
    {
        [SerializeField] [Range(0, 5)] float speed = 0.5f;
        [SerializeField] [Min(0)] float minDistance = 1f;

        private Transform target;
        private FighterCharacterController controller;

        private void Start()
        {
            controller = GetComponent<SF.FighterCharacterController>();
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        private void Update()
        {
            if (target && Vector3.Distance(target.position, transform.position) > minDistance) 
            {
                var dir = (target.position - transform.position).normalized;
                var input = dir * speed;
                controller.Move(new Vector2(input.x, input.z));
            }
            else
                controller.Move(Vector2.zero);
        }
    }
}