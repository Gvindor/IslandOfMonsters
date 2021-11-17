using System.Collections;
using UnityEngine;

namespace SF
{
    public class ComponentLinker : MonoBehaviour
    {
        [SerializeField] CombatController combatController;

        public CombatController CombatController => combatController;
    }
}