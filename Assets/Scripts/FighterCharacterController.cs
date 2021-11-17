﻿using RagdollMecanimMixer;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

namespace SF
{
    public class FighterCharacterController : MonoBehaviour
    {
        const string RagdollDefaultState = "default";
        const string RagdollDeadState = "dead";

        private Transform target;
        public Vector3 lookAtPos;
        public float lookAtTimer;
        public bool lookAt;

        [SerializeField] float rotationSpeed = 10;
        [SerializeField][Min(0)] float autoAttackDelay = 2f;
        [SerializeField][Min(0.1f)] float modeTransitionTime = 0.5f;

        private GameObject[] targets;

        private Rigidbody rb;
        private Animator animator;
        private RamecanMixer ragdoll;
        private Transform cam;

        private Vector3 input;
        private bool isAttacking;
        private float attackTimer;

        private float findTargetTimer;
        private bool isFocused;
        private bool isRun;
        private bool isDead;

        public bool IsAttacking => animator.GetCurrentAnimatorStateInfo(0).IsTag("attack");
        public bool IsDead => isDead;
        public Vector2 Input => input;
        public Transform Target => target;

        public UnityEvent OnAttack = new UnityEvent();

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            ragdoll = GetComponent<RamecanMixer>();
            //hitController = GetComponent<HitController>();
            cam = Camera.main.transform;
            //Time.timeScale = 0.3f;

            targets = GameObject.FindGameObjectsWithTag("Enemy");
        }

        public void Die()
        {
            isDead = true;
            ragdoll.BeginStateTransition(RagdollDeadState);
        }

        public void Move(Vector2 input)
        {
            if (!IsDead)
                this.input = new Vector3(input.x, 0, input.y);
            else 
                this.input = Vector3.zero;
        }

        public void Fight()
        {
            isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("attack");
            float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Default") || (isAttacking && time > 0.5f))
            {
                animator.SetBool("side", !animator.GetBool("side"));
                animator.SetInteger("number", Random.Range(0, 3));
                animator.SetTrigger("attack");

                attackTimer = autoAttackDelay;

                OnAttack.Invoke();
            }
        }

        private void AutoAttack()
        {
            if (target && attackTimer <= 0 && Vector3.Distance(transform.position, target.position) < 1)
            {
                Fight();
                attackTimer = autoAttackDelay;
            }

            attackTimer -= Time.deltaTime;
        }

        // Update is called once per frame
        void Update()
        {
            AutoAttack();

            Vector3 inputDirection = input.normalized;
            float inputVelocity = input.magnitude;

            Debug.DrawLine(transform.position, transform.position + input, Color.red);

            float angle = Vector3.SignedAngle(transform.forward, inputDirection, Vector3.up);

            var strafe = animator.GetFloat("strafe");
            if (target) //strafe mode
            {            
                strafe += Time.deltaTime / modeTransitionTime;

                Vector3 localInput = transform.InverseTransformDirection(input);

                animator.SetFloat("direction", localInput.x);
                animator.SetFloat("velocity", localInput.z);
            }
            else // run mode
            {
                strafe -= Time.deltaTime / modeTransitionTime;

                animator.SetFloat("direction", angle / 180);
                animator.SetFloat("velocity", inputVelocity);
            }
            animator.SetFloat("strafe", Mathf.Clamp01(strafe));

            Vector3 pos = transform.TransformPoint(new Vector3(0, 0.7f, 1));
            if (target != null)
            {
                pos = target.GetComponent<CombatController>().Head.position;
            }
            if (lookAtTimer > 0)
            {
                lookAtTimer -= Time.deltaTime;
                lookAtPos = Vector3.Lerp(lookAtPos, pos, (2 - lookAtTimer) / 2);
            }
            else
            {
                lookAtPos = pos;
            }
        }

        void LateUpdate()
        {
            if (target == null && isAttacking) return;
            //if (inputVelocity == 0) return;
            Vector3 directionToTarget = transform.forward;

            if (target == null)
            {
                if (input.magnitude > 0)
                    directionToTarget = input.normalized;
            }
            else
            {
                directionToTarget = target.position - rb.position;
            }
            Quaternion rotation = Quaternion.LookRotation(directionToTarget.normalized, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), Time.deltaTime * rotationSpeed);
        }

        public void ChangeTarget(Transform target)
        {
            this.target = target;
            lookAtTimer = 2;
        }

        void OnAnimatorIK()
        {
            if (lookAt)
            {
                animator.SetLookAtWeight(1, 0.5f);
                animator.SetLookAtPosition(lookAtPos);
            }
        }
    }
}