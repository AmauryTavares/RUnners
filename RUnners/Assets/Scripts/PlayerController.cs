using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 direction;
    public float speed;

    private int desiredLane = 1; // 0 = left, 1 = middle, 2 = right
    public float laneDistance; // Distance between each two lanes

    private int STOP_STATE = 0;
    private int PLAY_STATE = 1;
    private int PAUSE_STATE = 2;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameState() == PLAY_STATE)
        {
            direction.y = speed;
            HandlerInputKeyboard();

            if (gameManager.GetGameState() == PLAY_STATE)
            {
                transform.position += direction * Time.deltaTime;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Enemy")
        {
            PlayerDie();
        }

        if (col.collider.tag == "Restaurant")
        {
            gameManager.NextLevel();
        }
    }

    private void HandlerInputKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))// Handle input right
        {
            desiredLane++;

            if (desiredLane == 3)
            {
                desiredLane = 2;
            }

        } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))// Handle input left
        {
            desiredLane--;

            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        CalculateNextPosition(desiredLane);
    }

    private void CalculateNextPosition(int desiredLane) //Calculate and update next position
    {
        Vector3 targetPosition = transform.position.y * transform.right;
        
        if (desiredLane == 0) // Update target position to left
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2) // Update target position to right
        {
            targetPosition += Vector3.right * laneDistance;
        }

        if (transform.position == targetPosition)
        {
            return;
        } else
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moverDir = diff.normalized * 25 * Time.deltaTime;

            if (moverDir.sqrMagnitude < diff.sqrMagnitude)
            {
                transform.position += moverDir;
            } else
            {
                transform.position += diff;
            }
        }
    }

    public void PlayerDie()
    {
        if (gameManager.playerLifes > 1)
        {
            desiredLane = 1;
            transform.position = new Vector3();
        }

        gameManager.PlayerDie();
    }

    public float GetPlayerPosition()
    {
        return gameObject.transform.position.y;
    }
}
