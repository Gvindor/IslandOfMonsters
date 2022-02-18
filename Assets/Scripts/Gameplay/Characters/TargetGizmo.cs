using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SF
{
    public class TargetGizmo : MonoBehaviour
    {
        [SerializeField] CombatController target;
        [SerializeField] Image fill;

        private void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            if (target)
            {
                transform.position = target.transform.position;
                fill.fillAmount = target.HP / (float)target.MaxHP;

                if (target.Character.IsDead) Hide();
            }
        }

        public void Show(CombatController target)
        {
            gameObject.SetActive(true);
            this.target = target;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            target = null;
        }
    }
}