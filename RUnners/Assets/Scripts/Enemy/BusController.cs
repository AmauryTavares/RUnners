using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusController : MonoBehaviour
{
    public float speed;

    private Vector3 direction;

    private int PLAY_STATE = 1;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameState() == PLAY_STATE)
        {
            direction.y = -speed;
            transform.position += direction * Time.deltaTime;// Update movement forward
        }
    }
}
