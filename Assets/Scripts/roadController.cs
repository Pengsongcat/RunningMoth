using UnityEngine;

public class roadController : MonoBehaviour
{
    public GameObject roadPrefab;
    public float roadYFirst = 7.5f;
    public float roadLength = 20.829f; // road length
    public GameObject moth;

    private GameObject road1;
    private GameObject road2;

    private float nextMoveThreshold; 

    void Start()
    {
        road1 = Instantiate(roadPrefab, new Vector2(0, roadYFirst), Quaternion.identity);
        road2 = Instantiate(roadPrefab, new Vector2(0, roadYFirst + roadLength), Quaternion.identity);
        nextMoveThreshold = roadLength;
        // Debug.Log(nextMoveThreshold);
    }

    void Update()
    {
        if (moth == null) { return; }

        if (moth.transform.position.y > nextMoveThreshold)
        {
            MoveRoad();
            nextMoveThreshold += roadLength;
            // Debug.Log(nextMoveThreshold);
        }
    }

    void MoveRoad()
    {
        GameObject roadToMove = (road1.transform.position.y < road2.transform.position.y) ? road1 : road2;
        float newY = roadToMove.transform.position.y + 2 * roadLength;
        roadToMove.transform.position = new Vector3(roadToMove.transform.position.x, newY, roadToMove.transform.position.z);
    }

}

