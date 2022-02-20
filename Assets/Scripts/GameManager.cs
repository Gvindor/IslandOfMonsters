﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

            gameState = GameState.End;

            OnGameLost.Invoke();

            uiManager.SwithToLostScreen();
        }

        private void OnAllEnemiesDead()
        {
            spawner.OnCharacterDead.RemoveListener(OnCharacterDead);
            gameState = GameState.End;

            OnGameWon.Invoke();

            uiManager.SwithToWinScreen();
        }

        private enum GameState
        {
            Lobby,
            Gameplay,
            End
        }
    }
}