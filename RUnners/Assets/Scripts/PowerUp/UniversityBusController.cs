using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversityBusController : MonoBehaviour
{
    public float speed;
    public float limitDistance;

    private bool activeBus;
    private float currentDistance;
    private Vector3 direction;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = (Transform)GameObject.Find("Player").GetComponent(typeof(Transform));
        direction.y = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeBus == true)
        {
            transform.position += direction * Time.deltaTime;
            currentDistance += speed * Time.deltaTime;

            if (currentDistance >= limitDistance) // check limit distance
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                activeBus = false;
            }
        }

        float diff = playerTransform.position.y - transform.position.y;

        if (diff > 10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            activeBus = true;
        }
    }

    public bool GetActiveStatus()
    {
        return activeBus;
    }
    
}
