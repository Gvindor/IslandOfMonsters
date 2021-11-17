using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] string buttonName = "Fire";

        CrossPlatformInputManager.VirtualButton m_Button;

        void OnEnable()
        {
            if (!CrossPlatformInputManager.ButtonExists(buttonName))
            {
                m_Button = new CrossPlatformInputManager.VirtualButton(buttonName);
                CrossPlatformInputManager.RegisterVirtualButton(m_Button);
            }
            //else
            //{
            //    m_Button = CrossPlatformInputManager.VirtualAxisReference(axisName);
            //}
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            CrossPlatformInputManager.SetButtonDown(buttonName);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CrossPlatformInputManager.SetButtonUp(buttonName);
        }
    }
}
