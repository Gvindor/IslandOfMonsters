using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SF
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] ArenaManager arenaManager;
        [SerializeField] UIManager uiManager;
        [SerializeField] float endCutsceneLength = 10f;

        private PlayerSpawner spawner;
        private BoosterManager boosts;
        private CameraManager cameraManager;

        private GameState gameState;

        public UnityEvent OnGameStart = new UnityEvent();
        public UnityEvent OnGameWon = new UnityEvent();
        public UnityEvent OnGameLost = new UnityEvent();

        private void Awake()
        {
            arenaManager.OnArenaLoaded.AddListener(OnArenaLoaded);
            gameState = GameState.Lobby;
        }

        private void Start()
        {
            uiManager.SwithToLobby();
        }

        public void PrepareToStart()
        {
            uiManager.SwitchToGameplay();
            cameraManager.SwitchToGameplayCamera();
            gameState = GameState.Gameplay;
        }

        public void StartGame()
        {
            spawner.EnableEnemies();
            boosts.AutoSpawnEnabled = true;

            OnGameStart.Invoke();
        }

        public void NextGame()
        {
            arenaManager.LoadNextArena();
        }

        public void BackToLobby()
        {
            gameState = GameState.Lobby;
            uiManager.SwithToLobby();
            cameraManager.SwitchToOverviewCamera();

            NextGame();
        }

        private void OnArenaLoaded()
        {
            spawner = FindObjectOfType<PlayerSpawner>();
            spawner.OnCharacterDead.AddListener(OnCharacterDead);

            spawner.SpawnPlayer();
            spawner.SpawnEnemies();

            boosts = FindObjectOfType<BoosterManager>();
            boosts.AutoSpawnEnabled = false;

            cameraManager = FindObjectOfType<CameraManager>();
            cameraManager.UpdatePlayerReference();

            if (gameState == GameState.End)
            {
                uiManager.SwitchToGameplay();
                cameraManager.SwitchToGameplayCamera();
            }
            if (gameState == GameState.Lobby)
            {
                cameraManager.SwitchToOverviewCamera();
            }
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

            gameState = GameState.EndCutscene;

            OnGameLost.Invoke();

            StartCoroutine(EndGameAfterDelay(endCutsceneLength, false));
        }

        private void OnAllEnemiesDead()
        {
            spawner.OnCharacterDead.RemoveListener(OnCharacterDead);
            gameState = GameState.EndCutscene;

            spawner.Player?.Dance();

            cameraManager.SwitchToVictoryCamera();

            OnGameWon.Invoke();

            StartCoroutine(EndGameAfterDelay(endCutsceneLength, true));
        }

        private IEnumerator EndGameAfterDelay(float delay, bool win)
        {
            yield return new WaitForSecondsRealtime(delay);

            gameState = GameState.End;

            if (win)
                uiManager.SwithToWinScreen();
            else
                uiManager.SwithToLostScreen();
        }

        private enum GameState
        {
            Lobby,
            Gameplay,
            EndCutscene,
            End
        }
    }
}