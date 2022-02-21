using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] SoundEffect hitSfx;
        [SerializeField] SoundEffect punchSfx;
        [SerializeField] SoundEffect destructionSfx;
        [SerializeField] SoundEffect boostSfx;

        [SerializeField] AudioSource sourcePrefab;

        [SerializeField] int maxDestructionSounds = 10;

        private ObjectPool<AudioSource> sources;
        private static SoundManager instance;
        private List<AudioSource> destructionSources;

        private void Awake()
        {
            instance = this;

            sources = new ObjectPool<AudioSource>(transform, sourcePrefab);
            destructionSources = new List<AudioSource>();
        }

        public static void PlaySFX(SfxType sfxType, Vector3 position)
        {
            if (!instance)
            {
                Debug.LogError("Sound Manager instance is not found.");
                return;
            }

            SoundEffect sfx = null;
            switch (sfxType)
            {
                case SfxType.Hit:
                    sfx = instance.hitSfx;
                    break;
                case SfxType.Punch:
                    sfx = instance.punchSfx;
                    break;
                case SfxType.Destruction:
                    sfx = instance.destructionSfx;
                    break;
                case SfxType.Boost:
                    sfx = instance.boostSfx;
                    break;
            }

            if (sfx)
            {
                instance.PlaySfx(sfx, position);
            }
        }

        private void PlaySfx(SoundEffect sfx, Vector3 position)
        {
            if (sfx == destructionSfx && destructionSources.Count > instance.maxDestructionSounds) return;

            var source = sources.GetPooledObject();
            source.transform.position = position;

            var clip = sfx.GetRandomSFX();

            source.pitch = sfx.Pitch.Random();
            source.volume = sfx.Volume;
            source.clip = clip;
            source.loop = false;

            source.Play();

            if (sfx == destructionSfx) destructionSources.Add(source);

            StartCoroutine(RecycleSourceAfterDelay(source, clip.length));
        }

        private IEnumerator RecycleSourceAfterDelay(AudioSource source, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            sources.ReturnPooledObject(source);

            destructionSources.Remove(source);
        }

        public enum SfxType
        {
            Hit,
            Punch,
            Destruction,
            Boost
        }
    }
}