using System.Collections;
using UnityEngine;

namespace SF
{
    public class KillTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var linker = other.GetComponentInParent<ComponentLinker>();

            if (linker)
            {
                linker.CharacterController.Die();
            }
        }
    }
}