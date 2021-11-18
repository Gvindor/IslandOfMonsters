using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SF
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] CombatController target;
        [SerializeField] Image fill;

        private void LateUpdate()
        {
            if (!target) return;

            fill.fillAmount = target.HP / (float)target.MaxHP;
        }
    }
}