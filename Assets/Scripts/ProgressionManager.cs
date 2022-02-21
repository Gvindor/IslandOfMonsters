using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SF
{
    public class ProgressionManager : MonoBehaviour
    {
        private const float SkinUnlockThreshold = 0.97f; //After reaching this value the skin will be unlocked. Required to compensate any rounding errors.

        [SerializeField] CharacterSkin[] skins;
        [SerializeField] LevelConfig[] introlevels;
        [SerializeField] LevelConfig[] loopLevels;

        private int ActiveSkinIndex
        {
            get => PlayerPrefs.GetInt("skin_active", 0);
            set
            {
                PlayerPrefs.SetInt("skin_active", value);
                PlayerPrefs.Save();
            }
        }

        public float SkinProgress
        {
            get => PlayerPrefs.GetFloat("skin_progress", 0);
            private set
            {
                PlayerPrefs.SetFloat("skin_progress", value);
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

        public int CurrentLevelIndex
        {
            get => PlayerPrefs.GetInt("level", 0);
            private set
            {
                PlayerPrefs.SetInt("level", value);
                PlayerPrefs.Save();
            }
        }

        public bool SkinUnlocked => SkinProgress > SkinUnlockThreshold;
        public CharacterSkin NextSkin => skins[NextSkinIndex];
        public CharacterSkin ActiveSkin => skins[ActiveSkinIndex];

        public UnityEvent OnActiveSkinChanged = new UnityEvent();

        private void Awake()
        {
            var gm = FindObjectOfType<GameManager>();

            gm.OnGameWon.AddListener(OnGameWon);
            gm.OnGameLost.AddListener(OnGameLost);
        }

        private void OnGameWon()
        {
            float progress = SkinProgress;
            progress += GetCurrentLevelConfig().SkinProgressFillWin;

            if (progress > SkinUnlockThreshold)
            {
                progress = 1;
            }

            SkinProgress = progress;

            CurrentLevelIndex++;
        }

        private void OnGameLost()
        {
            float progress = SkinProgress;
            progress += GetCurrentLevelConfig().SkinProgressFillLoose;

            if (progress > SkinUnlockThreshold)
            {
                progress = 1;
            }

            SkinProgress = progress;
        }

        public void UseNextSkin()
        {
            ActiveSkinIndex = NextSkinIndex;
            SkinProgress = 0;

            int index = NextSkinIndex;
            index++;

            if (index == ActiveSkinIndex) index++;

            if (index >= skins.Length) index = 0;

            NextSkinIndex = index;

            OnActiveSkinChanged.Invoke();
        }

        public List<CharacterSkin> GetUnusedSkins()
        {
            var result = new List<CharacterSkin>(skins);
            result.Remove(ActiveSkin);

            return result;
        }

        public CharacterSkin[] GetEnemiesForLevel()
        {
            var enemies = new List<CharacterSkin>();
            var config = GetCurrentLevelConfig();
            var allowedSkins = GetUnusedSkins();

            enemies.AddRange(config.Enemies);

            //replace the skin used by the player
            int skinsToAdd = enemies.RemoveAll(s => s == ActiveSkin);
            if (skinsToAdd > 0)
            {
                foreach (var skin in allowedSkins)
                {
                    if (!enemies.Contains(skin))
                    {
                        for (int i = 0; i < skinsToAdd; i++)
                        {
                            enemies.Add(skin);
                        }

                        break;
                    }
                }
            }

            //add random enemies
            for (int i = 0; i < config.AdditionalRandomEnemies; i++)
            {
                int rndIndex = Random.Range(0, allowedSkins.Count);
                var skin = allowedSkins[rndIndex];
                enemies.Add(skin);
            }

            return enemies.ToArray();
        }

        public LevelConfig GetCurrentLevelConfig()
        {
            if (CurrentLevelIndex < introlevels.Length)
            {
                return introlevels[CurrentLevelIndex];
            }
            else
            {
                int index = (CurrentLevelIndex - introlevels.Length) % loopLevels.Length;
                return loopLevels[index];
            }
        }
    }
}