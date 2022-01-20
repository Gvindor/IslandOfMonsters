using System.Collections;
using UnityEngine;

namespace SF
{
    [System.Serializable]
    public class BoostData
    {
        [SerializeField] float duration = 5f;
        [SerializeField] float movementSpeedModifier = 1.2f;
        [SerializeField] float kickBackStrengthModifier = 1.2f;

        public float Duration => duration;
        public float Speed => movementSpeedModifier;
        public float KickBack => kickBackStrengthModifier;
    }
}