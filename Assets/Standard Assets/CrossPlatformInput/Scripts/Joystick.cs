using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		[SerializeField] RectTransform puck;
		[SerializeField] RectTransform background;

		[SerializeField] int MovementRange = 100;
		[SerializeField] int MinMovementRange = 10; //if joystick was moved less than the value it's considered to be a click
		[SerializeField] bool isCircleMovement = true;
		[SerializeField] string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		[SerializeField] string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
		[SerializeField] string clickButtonName = "Fire";

		bool m_isClick;
		Vector3 m_StartPos;
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualButton m_Button;

		void OnEnable()
		{
			CreateVirtualAxes();
		}

        void Start()
        {
            m_StartPos = puck.position;

			background.sizeDelta = new Vector2(MovementRange * 2, MovementRange * 2);

			puck.gameObject.SetActive(false);
			background.gameObject.SetActive(false);
		}

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;

			m_HorizontalVirtualAxis.Update(-delta.x);
			m_VerticalVirtualAxis.Update(delta.y);
		}

		void CreateVirtualAxes()
		{
			// create new axes
			if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
			{
				m_HorizontalVirtualAxis = CrossPlatformInputManager.VirtualAxisReference(horizontalAxisName);
			}
			else
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}

			if (CrossPlatformInputManager.AxisExists(verticalAxisName))
			{
				m_VerticalVirtualAxis = CrossPlatformInputManager.VirtualAxisReference(verticalAxisName);
			}
			else
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}

			// create a new button
			if (!CrossPlatformInputManager.ButtonExists(clickButtonName))
			{
				m_Button = new CrossPlatformInputManager.VirtualButton(clickButtonName);
				CrossPlatformInputManager.RegisterVirtualButton(m_Button);
			}
		}

		public void OnDrag(PointerEventData data)
		{
			Vector3 newPos = Vector3.zero;
			Vector2 delta = data.position - (Vector2)m_StartPos;

			if (delta.magnitude > MinMovementRange) m_isClick = false;

			if (isCircleMovement)
			{
				newPos = Vector3.ClampMagnitude(delta, MovementRange);
			}
			else 
			{
				newPos.x = Mathf.Clamp(delta.x, -MovementRange, MovementRange);
				newPos.y = Mathf.Clamp(delta.y, -MovementRange, MovementRange);
			}

			puck.position = m_StartPos + newPos;
			UpdateVirtualAxes(puck.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
			puck.position = m_StartPos;
			UpdateVirtualAxes(m_StartPos);

			if (m_isClick)
            {
                m_Button.Pressed();
                m_Button.Released();

                Debug.Log("Button click!");
			}

			puck.gameObject.SetActive(false);
			background.gameObject.SetActive(false);
		}


		public void OnPointerDown(PointerEventData data) 
		{
			m_StartPos = new Vector3(data.position.x, data.position.y);
			m_isClick = true;

			puck.position = m_StartPos;
			background.position = m_StartPos;

			puck.gameObject.SetActive(true);
			background.gameObject.SetActive(true);
		}

		void OnDisable()
		{
			// remove the joysticks from the cross platform input
			m_HorizontalVirtualAxis.Remove();
			m_VerticalVirtualAxis.Remove();
			// and the button
			m_Button.Remove();
		}
	}
}