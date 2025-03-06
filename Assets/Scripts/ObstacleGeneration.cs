using System.Collections.Generic;
using UnityEngine;

public class ObstacleGeneration : MonoBehaviour
{
    [Header("Player")]
    public GameObject moth;

    [Header("Obstacles")]
    public GameObject[] staticObjects;
    public GameObject[] dynamicObjects;

    [Header("Obstacles Setting")]
    [Tooltip("Offset of obstacles to moth in the Y-axis.")]
    public float obstacleOffset = 10f;
    [Tooltip("The initial Y position for the first obstacle.")]
    public float firstObstaclePosition = 5f;
    [Tooltip("The minimum X position for obstacles.")]
    public float minBound = 2f;
    [Tooltip("The maximum X position for obstacles.")]
    public float maxBound = 7f;
    [Tooltip("The interval of moth fly distance at which the difficulty level increases.")]
    public float difficultInterval = 100f;
    [Tooltip("The maximum difficulty level.")]
    public int maxDifficuty = 6;
    [Tooltip("The initial distance between every two obstacles.")]
    public float initialObstacleDistance = 20f;
    [Tooltip("The final max distance between every two obstacles after difficulty increases.")]
    public float finalObstacleDistance = 8f;

    private float mothY;
    private float nextObstacleY;
    private int difficultyLevel;
    private float obstacleDistance;

    private List<GameObject> obstacles = new List<GameObject>();    // store generated obstacles

    void Start()
    {
        nextObstacleY = firstObstaclePosition;
        difficultyLevel = 0;
        InvokeRepeating(nameof(CleanupObstacles), 2f, 2f);    // clean up obstacles every 2 seconds
    }

    void Update()
    {
        mothY = moth.transform.position.y;
        updateDifficuty();

        if (mothY >= nextObstacleY)
        {
            generateObstacle();
            float obstacleDistance = initialObstacleDistance - (initialObstacleDistance - finalObstacleDistance) * difficultyLevel / maxDifficuty;    // calculate obstacleDistance
            nextObstacleY += obstacleDistance;

            Debug.Log("Obstacle generated!");
            Debug.Log("Difficulty Level" + difficultyLevel);
        }
    }

    void updateDifficuty()
    {
        difficultyLevel = Mathf.Min(maxDifficuty, (int)(mothY / difficultInterval));    // update difficulty Level
    }

    private void generateObstacle()
    {
        /*if (difficultyLevel <= 2)       // low difficulty without dynamic obstacle
        {
            generateStatic();
        }
        else     // normal difficulty with dynamic obstacle
        {
            if (Random.value > 0.6f)
            {
                generateDynamic();
            }
            else
            {
                generateStatic();
            }
        }*/

        if (Random.value > 0.7f)
        {
            generateDynamic();
        }
        else
        {
            generateStatic();
        }
    }

    private void generateDynamic()
    {
        int objectIndex = Random.Range(0, dynamicObjects.Length);

        GameObject dynamicObject = Instantiate(dynamicObjects[objectIndex], new Vector2(11, mothY + obstacleOffset), Quaternion.identity);
        dynamicMovement objectScript = dynamicObject.GetComponent<dynamicMovement>();
        objectScript.isLeft = Random.value > 0.5f;   // face left/right by 0.5 chance

        obstacles.Add(dynamicObject);
    }

    private void generateStatic()
    {
        int objectIndex = Random.Range(0, staticObjects.Length);        // choose a random stastic object
        float posX = Random.value < 0.5f ? Random.Range(-maxBound, -minBound) : Random.Range(minBound, maxBound);           // choose a random position X

        if (objectIndex >= 0 && objectIndex < staticObjects.Length){
            GameObject staticObject = Instantiate(staticObjects[objectIndex], new Vector2(posX, mothY + obstacleOffset), Quaternion.identity);
            obstacles.Add(staticObject);
        }
    }

    private void CleanupObstacles()
    {
        float deleteThreshold = mothY - 5f;

        for (int i = obstacles.Count - 1; i >= 0; i--)  // reverse sequence
        {
            if (obstacles[i].transform.position.y < deleteThreshold)
            {
                Destroy(obstacles[i]); // delete GameObject
                obstacles.RemoveAt(i); // remove from list
            }
        }
    }

}
