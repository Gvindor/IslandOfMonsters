using System;

namespace SF
{
    [Serializable]
    public struct RangeFloat
    {
        public float min;
        public float max;

        public RangeFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Random()
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}