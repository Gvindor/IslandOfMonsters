using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SF
{
    public class CharacterProgressView : MonoBehaviour
    {
        [SerializeField] SceneReference previewScene;

        private void OnEnable()
        {
            SceneManager.LoadScene(previewScene, LoadSceneMode.Additive);
        }

        private void OnDisable()
        {
            SceneManager.UnloadSceneAsync(previewScene);
        }
    }
}