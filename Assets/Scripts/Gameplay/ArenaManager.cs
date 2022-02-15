using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SF
{
    [DefaultExecutionOrder(-10)]
    public class ArenaManager : MonoBehaviour
    {
        private ProgressionManager pm;
        private LevelConfig loadedLevel;

        public UnityEvent OnArenaLoaded = new UnityEvent();

        private void Awake()
        {
            pm = FindObjectOfType<ProgressionManager>();
        }

        private void Start()
        {
            LoadNextArena();
        }

        private IEnumerator LoadArena(LevelConfig config)
        {
            SceneManager.LoadScene(config.Scene, LoadSceneMode.Additive);
            loadedLevel = config;

            yield return null;

            var scene = SceneManager.GetSceneByPath(config.Scene);
            SceneManager.SetActiveScene(scene);

            OnArenaLoaded.Invoke();
        }

        public void LoadNextArena()
        {
            if (loadedLevel)
            {
                SceneManager.UnloadSceneAsync(loadedLevel.Scene);
            }

            var config = pm.GetCurrentLevelConfig();

            StartCoroutine(LoadArena(config));
        }

    }
}