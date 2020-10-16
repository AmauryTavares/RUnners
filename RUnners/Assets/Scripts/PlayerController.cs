using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerLifes;
    public float speed;
    public float cooldownStamina;
    public float stamina;
    public float laneDistance; // Distance between each two lanes
    public GameManager gameManager;
    public float timeEnergyDrink;

    public CameraController cameraController;

    private Vector3 direction;
    private float currentSpeed;
    private float currentStamina;
    private float currentCooldownStamina;
    private bool playerRun = false;
    private float currentTimeEnergyDrink;
    private bool activeEnergyDrink;
    private int currentPlayerLifes;
    private bool activeUniversityBus;
    private GameObject goUniversityBus;
    private UniversityBusController universityBusController;

    private int desiredLane; // 0 = left, 1 = middle, 2 = right

    private int STOP_STATE = 0;
    private int PLAY_STATE = 1;
    private int PAUSE_STATE = 2;


    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
        currentStamina = stamina;
        currentCooldownStamina = cooldownStamina;
        desiredLane = 1;
        activeEnergyDrink = false;
        activeUniversityBus = false;
        currentPlayerLifes = playerLifes;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameState() == PAUSE_STATE) 
        {
            gameObject.GetComponent<Animator>().speed = 0; // stop animation
        } else
        {
            gameObject.GetComponent<Animator>().speed = 1; // play animation
        }

        if (gameManager.GetGameState() == PLAY_STATE)
        {
            direction.y = currentSpeed;

            if (activeUniversityBus == false) // Gameplay player if not in university bus
            {
                HandlerInputKeyboard();
            }

            if (gameManager.GetGameState() == PLAY_STATE)
            {
                
                if (activeUniversityBus == true) // active university bus
                {
                    transform.GetComponent<BoxCollider2D>().enabled = false; // disable player collider
                    transform.position = goUniversityBus.transform.position;
                    cameraController.target = goUniversityBus.transform; // set target camera to university bus

                    if (universityBusController.GetActiveStatus() == false) // return to gameplay to player
                    {
                        gameManager.ActiveEnemies(true);
                        transform.GetComponent<BoxCollider2D>().enabled = true; // enable player collider
                        cameraController.target = gameObject.transform; // set target camera to player
                        desiredLane = 2; // move player to right
                        activeUniversityBus = false;
                    }

                } else // Update movement of player
                {
                    Vector3 movement = direction * Time.deltaTime;

                    if (activeEnergyDrink == true)
                    {
                        movement *= 1.5f;
                    }

                    transform.position += movement; // update position
                }

                if (activeEnergyDrink == true) // decreate time of energy drink
                {
                    currentTimeEnergyDrink -= Time.deltaTime;

                    if (currentTimeEnergyDrink <= 0) // check time of energy drink
                    {
                        activeEnergyDrink = false; // disable energy drink effect
                    }
                }

                if (playerRun == true) // decrease stamina
                {
                    currentStamina -= 10 * Time.deltaTime;

                    if (currentStamina < 0) // no stamina
                    {
                        currentSpeed /= 1.5f;
                        currentCooldownStamina = cooldownStamina;
                        playerRun = false;
                    }

                }
                else if (currentCooldownStamina <= 0) // increase stamina
                {
                    currentStamina += 5 * Time.deltaTime;

                    if (currentStamina > stamina) // set limit stamina
                    {
                        currentStamina = stamina;
                    }
                }
                else
                {
                    currentCooldownStamina -= Time.deltaTime;
                }
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


    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Puddle")
        {
            currentSpeed = speed / 2; // decrease speed in 50%
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Puddle")
        {
            currentSpeed = speed;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Dog")
        {
            PlayerDie();
        }

        if (col.tag == "Energy Drink")
        {
            activeEnergyDrink = true;
            currentTimeEnergyDrink = timeEnergyDrink;
            Destroy(col.gameObject);
        }

        if (col.tag == "Protein Bar")
        {
            playerLifes++;
            currentPlayerLifes++;
            Destroy(col.gameObject);
        }

        if (col.tag == "Acai Cup")
        {
            currentPlayerLifes = playerLifes;
            Destroy(col.gameObject);
        }

        if (col.tag == "University Bus")
        {
            goUniversityBus = col.gameObject;
            universityBusController = (UniversityBusController)goUniversityBus.GetComponent(typeof(UniversityBusController));
            gameManager.ActiveEnemies(false);
            activeUniversityBus = true;
        }
    }

    private void HandlerInputKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerRun == false)
        {
            if (currentStamina > 0)
            {
                currentSpeed *= 1.5f;
                playerRun = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && playerRun == true)
        {
            currentSpeed /= 1.5f;
            currentCooldownStamina = cooldownStamina;
            playerRun = false;
        }

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
        Vector3 targetPosition = transform.position.y * new Vector3(0, 1, 0);
        
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
        if (currentPlayerLifes > 1)
        {
            desiredLane = 1;
            transform.position = new Vector3();
            currentSpeed = speed;
            currentStamina = stamina;
            activeEnergyDrink = false;
            playerRun = false;
        }

        currentPlayerLifes--;

        gameManager.PlayerDie();
    }

    public float GetPlayerPosition()
    {
        return gameObject.transform.position.y;
    }

    public int GetPlayerLifes()
    {
        return currentPlayerLifes;
    }
}
