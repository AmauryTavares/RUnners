using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float goalLevel;
    public int startLevel;
    public string faseAtual;
    public string proximaFase;

    public TileManager tileManager;
    public PlayerController playerController;
    public GameObject player;

    public Image progressBar;

    private int START_STATE = 0;
    private int PLAY_STATE = 1;
    private int PAUSE_STATE = 2;
    private int NEXTLEVEL_STATE = 3;
    private int GAMEOVER_STATE = 4;

    private int gameState;
    private float cooldownMenu;
    private bool endTileSpawned;

    public GameObject pauseMenu;
    public GameObject gameOverPanel;
    public GameObject nextLevelPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameState = START_STATE;
        endTileSpawned = false;
        gameOverPanel.SetActive(false);
        nextLevelPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.fillAmount = player.transform.position.y / goalLevel;

        if (gameState == START_STATE)// if game state is start
        {
            if (playerController.GetPlayerLifes() > 0)
            {
                StartGame();
            }
            else
            {
                SetGameOverGame();
                GameOver();
            }
        }

        if (gameState == PLAY_STATE)
        {
            if (endTileSpawned == false && playerController.GetPlayerPosition() >= goalLevel) // Check if the player has reached the end of the level
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
        if (playerController.GetPlayerLifes() > 0) // Reset map only if player lifes > 0
        {
            tileManager.ResetMap();
        }

        SetStartGame();
    }

    public void SpawnEndTile() // Spawn last tile
    {
        tileManager.SpawnEndTile();
        endTileSpawned = true;
    }

    public void ActiveEnemies(bool value)
    {
        tileManager.SetActiveGenerateEnemy(value);
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

    public void SetPauseGame()
    {
        gameState = PAUSE_STATE;
        pauseMenu.SetActive(true);
    }

    public void SetPlayGame()
    {
        gameState = PLAY_STATE;
        pauseMenu.SetActive(false);
    }

    private void SetStartGame()
    {
        gameState = START_STATE;
    }

    public void SetNextLevelGame()
    {
        gameState = NEXTLEVEL_STATE;
        nextLevelPanel.SetActive(true);
    }

    private void SetGameOverGame()
    {
        gameState = GAMEOVER_STATE;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(faseAtual);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(proximaFase);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
