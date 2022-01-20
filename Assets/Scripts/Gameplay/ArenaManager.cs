using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SF
{
    [DefaultExecutionOrder(-10)]
    public class ArenaManager : MonoBehaviour
    {
        [SerializeField] SceneReference[] arenas;

        public UnityEvent OnArenaLoaded = new UnityEvent();

        private void Awake()
        {
#if UNITY_EDITOR
            if (GetLoadedArenaIndex() < 0)
            {
                StartCoroutine(LoadArena(0));
            }
#else
            StartCoroutine(LoadArena(0));
#endif
        }

        private int GetLoadedArenaIndex()
        {
            for (int i = 0; i < arenas.Length; i++)
            {
                var scene = SceneManager.GetSceneByPath(arenas[i].ScenePath);

                if (scene.isLoaded) return i;
            }

            return -1;
        }

        private IEnumerator LoadArena(int index)
        {
            SceneManager.LoadScene(arenas[index], LoadSceneMode.Additive);

            yield return null;

            var scene = SceneManager.GetSceneByPath(arenas[index]);
            SceneManager.SetActiveScene(scene);

            OnArenaLoaded.Invoke();
        }
    }
}