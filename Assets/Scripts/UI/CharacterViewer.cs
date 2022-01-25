using System.Collections;
using UnityEngine;

namespace SF
{
    public class CharacterViewer : MonoBehaviour
    {
        private void OnEnable()
        {
            var pm = FindObjectOfType<ProgressionManager>();

            if (!pm)
            {
                Debug.LogError("Can't find ProgressManager!", this);
                return;
            }

            var skin = pm.NextSkin;

            var inst = Instantiate(skin, transform);

            inst.GetComponentInChildren<PlayerInput>().enabled = false;
            inst.GetComponentInChildren<PlayerTargetSelector>().enabled = false;
            inst.GetComponentInChildren<TargetGizmo>().enabled = false;

            inst.transform.localPosition = Vector3.zero;
            inst.transform.localRotation = Quaternion.identity;

            //set preview layer
            foreach (Transform t in inst.GetComponentsInChildren<Transform>())
            {
                t.gameObject.layer = gameObject.layer;
            }
        }
    }
}