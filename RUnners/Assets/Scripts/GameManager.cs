﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerLifes;
    public float[] goalLevel;
    public int startLevel;

    public TileManager tileManager;
    public PlayerController playerController;

    private int START_STATE = 0;
    private int PLAY_STATE = 1;
    private int PAUSE_STATE = 2;
    private int NEXTLEVEL_STATE = 3;
    private int GAMEOVER_STATE = 4;

    private int gameState;
    private float cooldownMenu;
    private bool endTileSpawned;

    // Start is called before the first frame update
    void Start()
    {
        gameState = START_STATE;
        endTileSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == START_STATE)// if game state is start
        {
            if (playerLifes > 0)
            {
                StartGame();
            }
            else
            {
                GameOver();
            }
        }

        if (gameState == PLAY_STATE)
        {
            if (endTileSpawned == false && playerController.GetPlayerPosition() >= goalLevel[startLevel - 1]) // Check if the player has reached the end of the level
            {
                SpawnEndTile();
            }

            if (Input.GetKeyDown(KeyCode.Escape))// pause the game
            {
                SetPauseGame();
                cooldownMenu = 0.3f; // wait 0.3 seconds to press escape again
            }
        }

        if (gameState == PAUSE_STATE)
        {
            cooldownMenu -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Escape) && cooldownMenu <= 0)// back to game
            {
                SetPlayGame();
            }
        }
    }

    public void StartGame() // start the game from the beginning
    {
        tileManager.InitiateTiles();
        endTileSpawned = false;
        gameState = PLAY_STATE;
    }

    private void GameOver()
    {
        print("Game Over");
    }

    public void PlayerDie()
    {
        playerLifes--; 

        if (playerLifes > 0) // Reset map only if player lifes > 0
        {
            tileManager.ClearTiles();
            SetStartGame();
        } else
        {
            SetGameOverGame();
        }
    }

    public void SpawnEndTile() // Spawn last tile
    {
        tileManager.SpawnEndTile();
        endTileSpawned = true;
    }

    public void NextLevel()// Go to next level
    {
        SetNextLevelGame();
        print("Next Level");
    }

    public int GetGameState()
    {
        return gameState;
    }

    private void SetPauseGame()
    {
        gameState = PAUSE_STATE;
    }

    private void SetPlayGame()
    {
        gameState = PLAY_STATE;
    }

    private void SetStartGame()
    {
        gameState = START_STATE;
    }

    private void SetNextLevelGame()
    {
        gameState = NEXTLEVEL_STATE;
    }

    private void SetGameOverGame()
    {
        gameState = GAMEOVER_STATE;
    }

}