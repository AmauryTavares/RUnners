using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject[] enemyPrefabs;
    public GameObject tileClean;
    public GameObject tileRestaurant;
    public GameObject dogEnemy;
    public float cooldownSpawnDog;
    private float currentCooldownSpawnDog;
    private float ySpawn = 0;
    public float tileLength;
    public int numberOfTiles;

    private List<GameObject> activeTiles = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private GameObject activeDog;

    public Transform playerTransform;
    public GameManager gameManager;
    private bool endTileSpawned;
    private bool activeGenerateEnemy;

    private int PLAY_STATE = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldownSpawnDog = cooldownSpawnDog;
        endTileSpawned = false;
        activeGenerateEnemy = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (endTileSpawned == false 
            && gameManager.GetGameState() == PLAY_STATE 
            && playerTransform.position.y - tileLength > ySpawn - (numberOfTiles * tileLength)) // when the last tile is passed spawn new tile
        {
            SpawnTile();
            SpawnEnemy(Random.Range(0, enemyPrefabs.Length));
            DeleteTile();

            ySpawn += tileLength;
        }

        if (currentCooldownSpawnDog <= 0) // check cooldown to spawn dog
        {
            currentCooldownSpawnDog = cooldownSpawnDog; // reset cooldown to spawn dog

            if (activeDog == null )
            {
                //spawnDog(); // spawn dog
            }
        } else
        {
            currentCooldownSpawnDog -= Time.deltaTime; // decrease cooldown spawn dog
        }
    }

    private void SpawnEnemy(int Index) // Spawn new tile
    {
        GameObject go = Instantiate(enemyPrefabs[Index], transform.up * ySpawn, transform.rotation);
        activeEnemies.Add(go);

        go.SetActive(activeGenerateEnemy);
    }

    private void SpawnTile() // Spawn new tile
    {
        GameObject go = Instantiate(tileClean, transform.up * ySpawn, transform.rotation);
        activeTiles.Add(go);
    } 

    public void SpawnEndTile() // Spawn last tile
    {
        GameObject go = Instantiate(tileRestaurant, transform.up * ySpawn, transform.rotation);
        activeTiles.Add(go);

        ySpawn += tileLength;

        endTileSpawned = true;
    }

    private void DeleteTile() // Remove last Tile
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
        Destroy(activeEnemies[0]);
        activeEnemies.RemoveAt(0);
    }

    public void ResetMap()// Reset map
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            DeleteTile();
        }

        if (activeDog != null)
        {
            Destroy(activeDog);
            activeDog = null;
        }

        ySpawn = 0;
    }

    public void InitiateTiles() // Initiate firsts tiles
    {
        for (int i = 0; i < numberOfTiles; i++) // Spawn initials tiles
        {
            SpawnTile();

            if (i == 0)
            {
                activeEnemies.Add(new GameObject()); // null object
            } else
            {
                SpawnEnemy(Random.Range(0, enemyPrefabs.Length));
            }

            ySpawn += tileLength;
        }

        endTileSpawned = false;
    }

    public void SetActiveGenerateEnemy(bool value)
    {
        activeGenerateEnemy = value; // disable generation of enemies

        foreach (GameObject enemy in activeEnemies) // disable enemies spawned
        {
            enemy.SetActive(false);
        }
    }

    private void spawnDog()
    {
        activeDog = Instantiate(dogEnemy,  new Vector3(playerTransform.position.x, playerTransform.position.y - 4, playerTransform.position.z), transform.rotation);
    }
}
