using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SF
{
    public class Clickable : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClick = new UnityEvent();

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
        }
    }
}