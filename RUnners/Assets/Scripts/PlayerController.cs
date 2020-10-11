using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float speed;

    private int desiredLane = 1; // 0 = left, 1 = middle, 2 = right
    public float laneDistance; // Distance between each two lanes

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.y = speed;
        HandlerInputKeyboard();
    }

    private void FixedUpdate()
    {
        controller.Move(direction * Time.deltaTime);// Update movement forward
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
                controller.Move(moverDir);
            } else
            {
                controller.Move(diff);
            }
        }
    }

}
