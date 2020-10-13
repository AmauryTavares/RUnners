using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject[] tilePrefabs;
    public GameObject tileRestaurant;
    private float ySpawn = 0;
    public float tileLength;
    public int numberOfTiles;

    private List<GameObject> activeTiles = new List<GameObject>();

    public Transform playerTransform;
    public GameManager gameManager;
    private bool endTileSpawned;

    private int PLAY_STATE = 1;


    // Start is called before the first frame update
    void Start()
    {
        endTileSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (endTileSpawned == false 
            && gameManager.GetGameState() == PLAY_STATE 
            && playerTransform.position.y - tileLength > ySpawn - (numberOfTiles * tileLength)) // when the last tile is passed spawn new tile
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }
    }

    private void SpawnTile(int tileIndex) // Spawn new tile
    {
        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.up * ySpawn, transform.rotation);
        activeTiles.Add(go);

        ySpawn += tileLength;
    } 

    private void DeleteTile() // Remove last Tile
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    public void ClearTiles()// Reset map
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            DeleteTile();
        }

        ySpawn = 0;
    }

    public void InitiateTiles() // Initiate firsts tiles
    {
        for (int i = 0; i < numberOfTiles; i++) // Spawn initials tiles
        {
            if (i == 0)
            {
                SpawnTile(0);
            }
            else
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length));
            }
        }

        endTileSpawned = false;
    }

    public void SpawnEndTile() // Spawn last tile
    {
        GameObject go = Instantiate(tileRestaurant, transform.up * ySpawn, transform.rotation);
        activeTiles.Add(go);

        ySpawn += tileLength;

        endTileSpawned = true;
    }

}
