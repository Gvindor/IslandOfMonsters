using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SF
{
    public class ProgressionManager : MonoBehaviour
    {
        [SerializeField] int winsPerSkin = 3;
        [SerializeField] CharacterSkin[] skins;

        public int WinsPerSkin => winsPerSkin;

        private int ActiveSkinIndex
        {
            get => PlayerPrefs.GetInt("skin_active", 0);
            set
            {
                PlayerPrefs.SetInt("skin_active", value);
                PlayerPrefs.Save();
            }
        }

        public int SkinProgress
        {
            get => PlayerPrefs.GetInt("skin_progress", 0);
            private set
            {
                PlayerPrefs.SetInt("skin_progress", value);
                PlayerPrefs.Save();
            }
        }

        private int NextSkinIndex
        {
            get => skins.Length > 1 ? PlayerPrefs.GetInt("skin_next", 1) : 0;
            set
            {
                PlayerPrefs.SetInt("skin_next", value);
                PlayerPrefs.Save();
            }
        }

        public bool SkinUnlocked => SkinProgress == winsPerSkin;
        public CharacterSkin NextSkin => skins[NextSkinIndex];
        public CharacterSkin ActiveSkin => skins[ActiveSkinIndex];

        public UnityEvent OnActiveSkinChanged = new UnityEvent();

        private void Awake()
        {
            var gm = FindObjectOfType<GameManager>();

            gm.OnGameWon.AddListener(OnGameWon);
        }

        private void OnGameWon()
        {
            int progress = SkinProgress;
            progress++;

            if (progress > winsPerSkin)
            {
                progress = 1;

                int index = NextSkinIndex;
                index++;

                if (index == ActiveSkinIndex) index++; 

                if (index >= skins.Length) index = 0;

                NextSkinIndex = index;
            }

            SkinProgress = progress;
        }

        public void UseNextSkin()
        {
            ActiveSkinIndex = NextSkinIndex;

            OnActiveSkinChanged.Invoke();
        }

        public CharacterSkin[] GetUnusedSkins()
        {
            var result = new List<CharacterSkin>(skins);
            result.Remove(ActiveSkin);

            return result.ToArray();
        }
    }
}