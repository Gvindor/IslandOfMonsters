using UnityEngine;
using UnityEngine.UI;

namespace SF
{
    public class CharacterProgressView : MonoBehaviour
    {
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
            SetProgress(pm.SkinProgress);
            SetPreview(pm.ActiveSkin.Preview);
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