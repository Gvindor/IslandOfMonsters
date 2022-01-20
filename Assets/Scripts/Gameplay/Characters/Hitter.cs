using System.Collections;
using UnityEngine;

namespace SF
{
    public class Hitter : MonoBehaviour
    {
        [SerializeField] int hitDamage = 10;

        private ComponentLinker linker;

        public Vector3 Velocity
        {
            set
            {
                for (int i = 0; i < velocities.Length - 1; i++)
                {
                    velocities[i + 1] = velocities[i];
                }
                velocities[0] = value;
            }
            get
            {
                Vector3 avg = Vector3.zero;
                foreach (Vector3 vel in velocities)
                {
                    avg += vel;
                }
                avg /= velocities.Length;
                return avg;
            }
        }
        private Vector3[] velocities = new Vector3[2];
        private Vector3 oldPos;

        void Start()
        {
            linker = GetComponentInParent<ComponentLinker>();
        }

        void OnCollisionEnter(Collision col)
        {
            //if (col.impulse == Vector3.zero) return;
            Transform targetParent = col.transform.parent;
            if (targetParent == null || targetParent.name != "Ragdoll" || targetParent == transform.parent) return;

            ComponentLinker targetLinker = col.transform.GetComponentInParent<ComponentLinker>();
            CombatController targetCombatController = targetLinker.CombatController;

            if (!linker.CombatController.CanHitThisTarget(targetCombatController)) return;

            linker.CombatController.GiveHit(targetCombatController, col.gameObject, col.contacts[0].point, Velocity, hitDamage);
        }

        private void FixedUpdate()
        {
            Velocity = (transform.position - oldPos) / Time.fixedDeltaTime;
            oldPos = transform.position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.05f);
        }
    }
}