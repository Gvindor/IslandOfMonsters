using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SF
{
    public class CharacterProgressView : MonoBehaviour
    {
        [SerializeField] TMP_Text progressLabel;
        [SerializeField] Image previewImage;
        [SerializeField] Image previewShadow;
        [SerializeField] GameObject skinInUse;

        private ProgressionManager pm;

        private void Awake()
        {
            pm = FindObjectOfType<ProgressionManager>();
        }

        private void OnEnable()
        {
            SetProgress(pm.SkinProgress / (float)pm.WinsPerSkin);
            SetPreview(pm.ActiveSkin.Preview);

            progressLabel.text = $"{pm.SkinProgress} / {pm.WinsPerSkin}";

            progressLabel.gameObject.SetActive(!pm.SkinUnlocked);
        }

        private void SetPreview(Sprite sprite)
        {
            previewImage.sprite = sprite;
            previewShadow.sprite = sprite;
        }

        private void SetProgress(float progress)
        {
            previewImage.fillAmount = progress;
        }
    }
}