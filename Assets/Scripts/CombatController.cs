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

        private FighterCharacterController controller;
        private Rigidbody rb;
        private List<CombatController> hitTargets = new List<CombatController>();

        private ParticleSystem hitParticles;

        public int HP { get; set; }

        public Transform Head => head;

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

        public bool TakeHit(GameObject go, Vector3 point, Vector3 impulse, int damage)
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

            Rigidbody boneRb = go.GetComponent<Rigidbody>();
            boneRb.AddForceAtPosition(impulse.normalized * hitImpulse, point, ForceMode.Impulse);
            Vector3 dir = new Vector3(impulse.x, 0, impulse.z);
            rb.AddForce(dir.normalized * 400, ForceMode.Impulse);

            PlayHitVFX(point);

            return true;
        }

        public bool CanHitThisTarget(CombatController targetHitController)
        {
            if (!controller) return false;

            if (!controller.IsAttacking || controller.IsDead) return false;

            return !hitTargets.Contains(targetHitController);
        }

        public void GiveHit(CombatController targetHitController)
        {
            hitTargets.Add(targetHitController);
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