using UnityEngine;

namespace SF
{
    [CreateAssetMenu(menuName = "Sound Effect")]
    public class SoundEffect : ScriptableObject
    {
        [SerializeField] AudioClip[] sfx;
        [SerializeField] RangeFloat pitch = new RangeFloat(1f, 1f);
        [SerializeField] float volume = 1f;

        public RangeFloat Pitch => pitch;
        public float Volume => volume;

        public AudioClip GetRandomSFX()
        {
            int index = Random.Range(0, sfx.Length);

            return sfx[index];
        }
    }
}