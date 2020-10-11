using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject[] tilePrefabs;
    public float ySpawn;
    public float tileLength;
    public int numberOfTiles;

    private List<GameObject> activeTiles = new List<GameObject>();

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {  
        for (int i = 0; i < numberOfTiles; i++) // Spawn initials tiles
        {
            if (i == 0)
            {
                SpawnTile(0);
            } else
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length));
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.y - tileLength > ySpawn - (numberOfTiles * tileLength)) // when the last tile is passed spawn new tile
        {
            SpawnTile(0);
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
}
