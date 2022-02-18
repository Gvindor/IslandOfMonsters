using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class LevelProgressView : MonoBehaviour
    {
        [SerializeField] LevelPuck puckPrefab;
        [SerializeField] RectTransform linePrefab;
        [SerializeField] int levelsNum = 3;
        [SerializeField] float distanceBetweenPucks = 200;

        private ProgressionManager pm;

        private void Awake()
        {
            pm = FindObjectOfType<ProgressionManager>();
        }

        private void OnEnable()
        {
            int currentLevel = pm.CurrentLevelIndex + 1;
            int fromLevel = currentLevel - levelsNum;
            int toLevel = currentLevel + levelsNum;

            if (fromLevel < 1) fromLevel = 1;

            float offset = (currentLevel - fromLevel) * distanceBetweenPucks;

            CreateLines(toLevel - fromLevel, offset);
            CreatePucks(fromLevel, toLevel, currentLevel, offset);
        }

        private void OnDisable()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void CreateLines(int quantity, float offset)
        {
            for (int i = 0; i < quantity; i++)
            {
                var line = Instantiate(linePrefab, transform);
                var size = line.sizeDelta;
                size.x = distanceBetweenPucks;
                line.sizeDelta = size;

                line.localPosition = new Vector3(i * distanceBetweenPucks - offset + distanceBetweenPucks * 0.5f, 0, 0);
            }
        }

        private void CreatePucks(int fromLevel, int toLevel, int activeLevel, float offset)
        {
            int count = 0;
            for (int i = fromLevel; i <= toLevel; i++)
            {
                var puck = Instantiate(puckPrefab, transform);

                puck.Text = i.ToString();

                if (i < activeLevel)
                    puck.State = LevelPuck.StateEnum.Passed;
                else if (i > activeLevel)
                    puck.State = LevelPuck.StateEnum.Locked;
                else
                    puck.State = LevelPuck.StateEnum.Active;

                puck.transform.localPosition = new Vector3(count * distanceBetweenPucks - offset, 0, 0);

                count++;
            }
        }
    }
}