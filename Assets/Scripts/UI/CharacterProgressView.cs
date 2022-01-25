using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace SF
{
    public class CharacterProgressView : MonoBehaviour
    {
        [SerializeField] SceneReference previewScene;
        [SerializeField] TMP_Text progressLabel;
        [SerializeField] Button useButton;
        [SerializeField] GameObject skinInUse;

        private ProgressionManager pm;

        private void Awake()
        {
            useButton.onClick.AddListener(UseCurrentSkin);

            pm = FindObjectOfType<ProgressionManager>();
        }

        private void OnEnable()
        {
            SceneManager.LoadScene(previewScene, LoadSceneMode.Additive);

            progressLabel.text = $"{pm.SkinProgress} / {pm.WinsPerSkin}";

            progressLabel.gameObject.SetActive(!pm.SkinUnlocked);
            useButton.gameObject.SetActive(pm.SkinUnlocked);
        }

        private void OnDisable()
        {
            SceneManager.UnloadSceneAsync(previewScene);
        }

        private void UseCurrentSkin()
        {
            if (pm.SkinUnlocked)
            {
                pm.UseNextSkin();
                useButton.gameObject.SetActive(false);

                if (skinInUse)
                    skinInUse.SetActive(true);
            }
        }
    }
}