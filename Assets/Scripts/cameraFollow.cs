using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public GameObject moth;
    public float posOffset = 3.5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moth != null)
        {
            Vector3 pos = transform.position;
            pos.y = posOffset + moth.transform.position.y;
            transform.position = pos;
        }
    }
}
