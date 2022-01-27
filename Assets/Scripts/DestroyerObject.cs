using UnityEngine;

namespace IslandioPrototype
{
    public class DestroyerObject : MonoBehaviour
    {
        [SerializeField] private GameObject SwapObject;
        //[SerializeField] private float timeRemaining = 5; //время после которого должен удалится объект после разрушения (сделано во благо оптимизации)

        private void OnTriggerEnter(Collider other)
        {
            //TODO: fix this
            //if (other.gameObject.GetComponent<LineOfSplit>() || other.gameObject.GetComponent<MeteorExplosion>())
            //{
            //    GameObject Oskolki = Instantiate(SwapObject, transform.position, transform.rotation) as GameObject;
            //    //Destroy(Oskolki, timeRemaining);
            //    Destroy(gameObject);
            //}
        }

        private void OnCollisionEnter(Collision collision)
        {
            //TODO: fix this
            //if (collision.gameObject.GetComponent<MeteorExplosion>())
            //{
            //    GameObject Oskolki = Instantiate(SwapObject, transform.position, transform.rotation) as GameObject;
            //    //Destroy(Oskolki, timeRemaining);
            //    Destroy(gameObject);
            //}
        }
    }
}

