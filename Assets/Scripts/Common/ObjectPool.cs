using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private T prefab;
        private Transform parent;

        Stack<T> pool = new Stack<T>();

        public ObjectPool(Transform parent, T prefab)
        {
            this.parent = parent;
            this.prefab = prefab;
        }

        public T GetPooledObject()
        {
            T instance = null;

            if (pool.Count > 0)
            {
                instance = pool.Pop();
            }
            else
            {
                instance = Object.Instantiate(prefab);
                instance.transform.SetParent(parent, false);
            }

            instance.gameObject.SetActive(true);

            return instance;
        }

        public void ReturnPooledObject(T instance)
        {
            if (!pool.Contains(instance)) 
            {
                pool.Push(instance);
                instance.gameObject.SetActive(false);
            }
        }
    }
}