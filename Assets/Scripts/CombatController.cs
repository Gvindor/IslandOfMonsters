using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class CombatController : MonoBehaviour
    {
        [SerializeField] Transform head;
        [SerializeField] GameObject[] hitBodyParts;
        [SerializeField] int maxHP = 100;
        [SerializeField] float hitImpulse = 50;
        [SerializeField] ParticleSystem hitParticlesPrefab;
        [SerializeField] bool powerKicksEnabled;
        [SerializeField] [Range(0, 1)] float powerKickChance = 0.25f;
        [SerializeField] bool bulletTimeOnHitRecived;
        [SerializeField] bool bulletTimeOnHitGiven;

        private FighterCharacterController controller;
        private Rigidbody rb;
        private List<CombatController> hitTargets = new List<CombatController>();

        private ParticleSystem hitParticles;

        public int HP { get; set; }
        public int MaxHP => maxHP;

        public Transform Head => head;
        public FighterCharacterController Character => controller;

        private void Awake()
        {
            HP = maxHP;

            controller = GetComponent<FighterCharacterController>();
            rb = GetComponent<Rigidbody>();

            controller.OnAttack.AddListener(OnAttackStarted);
        }

        private void OnAttackStarted()
        {
            hitTargets.Clear();
        }

        private bool TakeHit(GameObject go, Vector3 point, Vector3 impulse, int damage, bool isPowerKick)
        {
            if (controller.IsDead) return false;

            Debug.Log("Got hit!");

            bool hit = false;
            foreach (GameObject bone in hitBodyParts)
            {
                if (go == bone)
                {
                    hit = true;
                    break;
                }
            }
            if (!hit) return false;

            HP -= damage;

            if (HP <= 0)
            {
                HP = 0;
                controller.Die();
            }

            float kickBackImpulse = isPowerKick ? hitImpulse * 5 : hitImpulse;

            Rigidbody boneRb = go.GetComponent<Rigidbody>();
            boneRb.AddForceAtPosition(impulse.normalized * kickBackImpulse, point, ForceMode.Impulse);

            if (isPowerKick)
            {
                controller.Faint();

                if (bulletTimeOnHitRecived)
                    FindObjectOfType<TimeManager>().BulletTime();
            }

            //Vector3 dir = new Vector3(impulse.x, 0, impulse.z);
            //rb.AddForce(dir.normalized * 400, ForceMode.Impulse);

            PlayHitVFX(point);



            return true;
        }

        public bool CanHitThisTarget(CombatController targetHitController)
        {
            if (!controller) return false;

            if (!controller.IsAttacking || controller.IsDead) return false;

            return !hitTargets.Contains(targetHitController);
        }

        public void GiveHit(CombatController targetController, GameObject targetBodyPart, Vector3 hitPosition, Vector3 hitImpulse, int baseDamage)
        {
            bool isPowerKick = false;
            if (powerKicksEnabled) 
            {
                var dice = Random.Range(0, 1f);
                isPowerKick = dice <= powerKickChance;
            }

            if (targetController.TakeHit(targetBodyPart, hitPosition, hitImpulse, baseDamage, isPowerKick))
            {
                hitTargets.Add(targetController);

                if (isPowerKick && bulletTimeOnHitGiven)
                    FindObjectOfType<TimeManager>().BulletTime();
            }
        }

        private void PlayHitVFX(Vector3 position)
        {
            if (!hitParticles)
            {
                hitParticles = Instantiate(hitParticlesPrefab, position, Quaternion.identity, transform);
            }

            hitParticles.transform.position = position;

            hitParticles.Emit(20);
        }
    }
}