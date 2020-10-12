using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyList;
    public float spawnFrequency;
    private float counterSpawnEnemy;
    public float offsetSpawn;
    public float laneDistance; // Distance between each two lanes

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        counterSpawnEnemy = spawnFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        counterSpawnEnemy -= Time.deltaTime;

        if (counterSpawnEnemy <= 0)
        {
            spawnStaticEnemy(Random.Range(0, enemyList.Length), Random.Range(0, 3));

            counterSpawnEnemy = spawnFrequency; // Reset counter
        }
    }

    private void spawnStaticEnemy(int enemyIndex, int line)
    {
        Vector3 targetPosition = transform.up * (playerTransform.position.y + offsetSpawn);

        if (line == 0) // Update target position to left
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (line == 2) // Update target position to right
        {
            targetPosition += Vector3.right * laneDistance;
        }

        Instantiate(enemyList[enemyIndex], targetPosition, transform.rotation);
    }
}
