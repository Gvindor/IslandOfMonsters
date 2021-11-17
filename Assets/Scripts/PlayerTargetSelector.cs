﻿using System.Collections;
using UnityEngine;

namespace SF
{
    public class PlayerTargetSelector : MonoBehaviour
    {
        [SerializeField] [Min(0)] float enemyLockDistance = 2f;
        [Tooltip("How long character should run from the target before breaking lock")]
        [SerializeField] [Min(0)] float retreatTime = 1f;

        private FighterCharacterController character;
        private float findTargetTimer;
        private float retreatTimer;

        private GameObject[] targets;

        private void Awake()
        {
            character = GetComponent<FighterCharacterController>();
            targets = GameObject.FindGameObjectsWithTag("Enemy");
        }

        private void Update()
        {
            RetreatCheck();

            if (!character.IsAttacking)
                FindTarget();
        }

        private void FindTarget()
        {
            if (findTargetTimer > 0)
            {
                findTargetTimer -= Time.deltaTime;
            }
            else
            {
                if (targets.Length == 0)
                    targets = GameObject.FindGameObjectsWithTag("Enemy");
                float dist = float.MaxValue;
                int id = -1;

                for (int i = 0; i < targets.Length; i++)
                {
                    Vector3 dir = targets[i].transform.position - transform.position;
                    //float tempDot = inputVelocity == 0 ? 1 : Vector3.Dot(inputDirection, dir);
                    //tempDot > 0.75 && 
                    if (dir.magnitude < dist && dir.magnitude < enemyLockDistance)
                    {
                        if (!targets[i].GetComponent<SF.FighterCharacterController>().IsDead)
                        {
                            dist = dir.magnitude;
                            id = i;
                        }
                    }
                }

                bool isFocused = id != -1;
                if (isFocused)
                    character.ChangeTarget(targets[id].transform);
                else
                    character.ChangeTarget(null);

                findTargetTimer = 0.5f;
            }
        }

        private void RetreatCheck()
        {
            if (character.Target)
            {
                if (character.Input.magnitude > 0.8f) //we are moving fast
                {
                    var targetDir3D = character.Target.position - transform.position;
                    var targetDir = new Vector2(targetDir3D.x, targetDir3D.z).normalized;

                    if (Vector2.Angle(character.Input.normalized, targetDir) > 90) //we are running away from the target
                    {
                        retreatTimer -= Time.deltaTime;

                        if (retreatTimer <= 0)
                        {
                            findTargetTimer = 2f;
                            character.ChangeTarget(null);
                        }
                    }
                    else
                        retreatTimer = retreatTime;
                }
            }
            else
                retreatTimer = retreatTime;
        }
    }
}