using System.Collections;
using UnityEngine;
using TMPro;

namespace SF
{
    [RequireComponent(typeof(TMP_Text))]
    public class KillCounter : MonoBehaviour
    {
        private TMP_Text label;
        private PlayerSpawner spawner;

        private void Awake()
        {
            label = GetComponent<TMP_Text>();
            spawner = FindObjectOfType<PlayerSpawner>();
        }

        private void OnEnable()
        {
            spawner?.OnCharacterSpawned.AddListener(OnCharactersChanged);
            spawner?.OnCharacterDead.AddListener(OnCharactersChanged);

            OnCharactersChanged(null);
        }

        private void OnDisable()
        {
            spawner?.OnCharacterSpawned.RemoveListener(OnCharactersChanged);
            spawner?.OnCharacterDead.RemoveListener(OnCharactersChanged);
        }

        private void OnCharactersChanged(FighterCharacterController character)
        {
            label.text = $"{spawner.AliveEnemiesCount} / {spawner.TotalCharacters - 1}";
        }
    }
}