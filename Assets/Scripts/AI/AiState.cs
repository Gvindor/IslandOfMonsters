using UnityEngine;

namespace SF.AI
{
    public abstract class AiState : MonoBehaviour
    {
        public void TransitionToState(AiState state)
        {
            this.gameObject.SetActive(false);
            state.gameObject.SetActive(true);
        }
    }
}