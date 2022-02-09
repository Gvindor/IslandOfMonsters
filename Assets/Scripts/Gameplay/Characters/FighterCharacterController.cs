using RagdollMecanimMixer;
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

        [SerializeField][Tooltip("In local space")] Vector3 defaultLoopAtPos = new Vector3(0, 1f, 2f);

        [SerializeField] float rotationSpeed = 10;
        [SerializeField] [Min(0)] float autoAttackDelay = 2f;
        [SerializeField] [Min(0)] float autoAttackRange = 1f;
        [SerializeField] [Min(0.1f)] float modeTransitionTime = 0.5f;
        [SerializeField] [Min(0)] float faintDuration = 2f;

        [SerializeField] Renderer characterRenderer;
        [SerializeField] ParticleSystem powerUpPrefab;

        private GameObject[] targets;

        private Rigidbody rb;
        private Animator animator;
        private RamecanMixer ragdoll;
        private Transform cam;
        private ParticleSystem powerUpVFX;

        private Vector3 input;
        private float attackTimer;
        private float faintTimer;
        private float boostTimer;

        private float findTargetTimer;
        private bool isFocused;
        private bool isRun;
        private bool isDead;
        private bool isFainted;

        public bool IsAttacking => animator.GetCurrentAnimatorStateInfo(0).IsTag("attack");
        public bool IsDead => isDead;
        public bool IsFainted => isFainted;
        public Vector2 Input => input;
        public Transform Target => target;
        public float SpeedModifier { get; set; } = 1f;
        public BoostData ActiveBoost { get; private set; }
        public float BoostTimeLeft => boostTimer;

        public UnityEvent OnAttack = new UnityEvent();
        public UnityEvent<FighterCharacterController, Transform> OnTargetChanged = new UnityEvent<FighterCharacterController, Transform>();
        public UnityEvent OnDead = new UnityEvent();

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            ragdoll = GetComponent<RamecanMixer>();
            cam = Camera.main.transform;

            targets = GameObject.FindGameObjectsWithTag("Enemy");
        }

        public void Die()
        {
            isDead = true;

            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            ragdoll.BeginStateTransition(RagdollDeadState);
            animator.SetBool("dead", true);

            var navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent) navAgent.enabled = false;

            ClearBoost();

            OnDead.Invoke();
        }

        public void Faint()
        {
            isFainted = true;
            faintTimer = faintDuration;

            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            ragdoll.BeginStateTransition(RagdollDeadState);
            animator.SetBool("dead", true);
        }

        public void Revive()
        {
            isDead = false;
            isFainted = false;

            rb.position = ragdoll.RootBoneTr.position;
            rb.isKinematic = false;
            GetComponent<Collider>().enabled = true;

            ragdoll.BeginStateTransition(RagdollDefaultState);

            animator.SetBool("dead", false);
            animator.SetTrigger("reviveUp");
        }

        public void Move(Vector2 input)
        {
            if (!IsDead && !IsFainted)
                this.input = new Vector3(input.x, 0, input.y);
            else 
                this.input = Vector3.zero;
        }

        public void Fight()
        {
            float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Default") || (IsAttacking && time > 0.5f))
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
            if (target && attackTimer <= 0 && Vector3.Distance(transform.position, target.position) < autoAttackRange)
            {
                Fight();
                attackTimer = autoAttackDelay;
            }

            attackTimer -= Time.deltaTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (IsFainted && !IsDead)
            {
                faintTimer -= Time.deltaTime;

                if (faintTimer < 0)
                {
                    Revive();
                }
                else
                {
                    rb.position = ragdoll.RootBoneTr.position;
                }
            }

            if (boostTimer > 0) boostTimer -= Time.deltaTime;
            if (boostTimer < 0 && ActiveBoost != null) ClearBoost();

            if (IsDead || IsFainted) return;

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

                animator.SetFloat("direction", localInput.x * SpeedModifier);
                animator.SetFloat("velocity", localInput.z * SpeedModifier);
            }
            else // run mode
            {
                strafe -= Time.deltaTime / modeTransitionTime;

                animator.SetFloat("direction", angle / 180);
                animator.SetFloat("velocity", inputVelocity * SpeedModifier);
            }
            animator.SetFloat("strafe", Mathf.Clamp01(strafe));

            Vector3 pos = transform.TransformPoint(defaultLoopAtPos);
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
            //if (target == null && IsAttacking) return;

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

            OnTargetChanged.Invoke(this, target);
        }

        public void ApplyBoost(BoostData boost)
        {
            ActiveBoost = boost;
            boostTimer = boost.Duration;
            animator.speed = boost.Speed;

            characterRenderer.material.SetFloat("_FresnelPower", 1);

            if (!powerUpVFX)
                powerUpVFX = Instantiate(powerUpPrefab, transform);

            powerUpVFX.Play();
        }

        public void ClearBoost()
        {
            animator.speed = 1f;
            boostTimer = 0;
            ActiveBoost = null;

            characterRenderer.material.SetFloat("_FresnelPower", 0);

            powerUpVFX?.Stop();
        }

        void OnAnimatorIK()
        {
            if (lookAt)
            {
                animator.SetLookAtWeight(1, 0.5f);
                animator.SetLookAtPosition(lookAtPos);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }
}