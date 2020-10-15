using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public float speed;

    private bool dogRun;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = (Transform) GameObject.Find("Player").GetComponent(typeof(Transform));
        dogRun = false;
    }

    // Update is called once per frame
    void Update()
    {
        float diff = playerTransform.position.y - transform.position.y;

        if (diff > 4) // distance between dog and player active dog script
        {
            dogRun = true;
        }

        if (dogRun == true)
        {
            Vector3 direction = new Vector3(0, speed, 0);
            Vector3 line = new Vector3(playerTransform.position.x - transform.position.x, 0, 0);
            transform.position += line * 10 * Time.deltaTime;
            transform.position += direction * Time.deltaTime; // runs towards the player

            if (diff > 8)
            {
                Destroy(gameObject);
            }
        }
    }
}
