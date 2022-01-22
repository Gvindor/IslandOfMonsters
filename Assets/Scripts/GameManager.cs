using System.Collections;
using UnityEngine;

namespace SF
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] ArenaManager arenaManager;
        [SerializeField] UIManager uiManager;

        private PlayerSpawner spawner;
        private BoosterManager boosts;
        private CameraManager cameraManager;

        private GameState gameState;

        private void Awake()
        {
            arenaManager.OnArenaLoaded.AddListener(OnArenaLoaded);

            uiManager.SwithToLobby();

            gameState = GameState.Lobby;
        }

        public void StartGame()
        {
            boosts.AutoSpawnBoosts = true;
            spawner.SpawnEnemies();
            uiManager.SwitchToGameplay();

            gameState = GameState.Gameplay;
        }

        public void NextGame()
        {
            arenaManager.LoadNextArena();
        }

        private void OnArenaLoaded()
        {
            spawner = FindObjectOfType<PlayerSpawner>();
            spawner.OnCharacterDead.AddListener(OnCharacterDead);

            spawner.SpawnPlayer();

            boosts = FindObjectOfType<BoosterManager>();
            boosts.AutoSpawnBoosts = false;

            cameraManager = FindObjectOfType<CameraManager>();
            cameraManager.UpdatePlayerReference();

            if (gameState == GameState.End)
                StartGame();
        }

        private void OnCharacterDead(FighterCharacterController character)
        {
            if (character.CompareTag("Player"))
                OnPlayerDead();
            else
            {
                if (spawner && spawner.AliveEnemiesCount == 0)
                {
                    OnAllEnemiesDead();
                }
            }
        }

        private void OnPlayerDead()
        {
            spawner.OnCharacterDead.RemoveListener(OnCharacterDead);

            uiManager.SwithToLostScreen();

            gameState = GameState.End;
        }

        private void OnAllEnemiesDead()
        {
            spawner.OnCharacterDead.RemoveListener(OnCharacterDead);
            uiManager.SwithToWinScreen();

            gameState = GameState.End;
        }

        private enum GameState
        {
            Lobby,
            Gameplay,
            End
        }
    }
}