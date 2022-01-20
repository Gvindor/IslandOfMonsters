using System.Collections;
using UnityEngine;

namespace SF
{
    public class ComponentLinker : MonoBehaviour
    {
        [SerializeField] FighterCharacterController characterController;
        [SerializeField] CombatController combatController;

        public FighterCharacterController CharacterController => characterController;
        public CombatController CombatController => combatController;
    }
}