using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace SF 
{ 
    public class PlayerInput : MonoBehaviour
    {
        private FighterCharacterController controller;

        private void Awake()
        {
            controller = GetComponent<FighterCharacterController>();
        }

        private void Update()
        {
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            var input = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0) * new Vector3(horizontal, 0, vertical);
            var move = new Vector2(input.x, input.z);

            controller.Move(move);

            if (CrossPlatformInputManager.GetButtonDown("Fire"))
            {
                controller.Fight();
            }
        }
    }
}