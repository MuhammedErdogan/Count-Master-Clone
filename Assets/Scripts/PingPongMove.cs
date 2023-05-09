using UnityEngine;

public class PingPongMove : MonoBehaviour
{
    public float speed = 1.0f; // speed of movement
    public float leftBorder = -10.0f; // left border of movement
    public float rightBorder = 10.0f; // right border of movement

    private float journeyLength;
    private float startTime;

    void Start()
    {
        journeyLength = Mathf.Abs(leftBorder - rightBorder);

        // Set a random starting position within the borders
        float randomStartPos = Random.Range(leftBorder, rightBorder);
        transform.position = new Vector3(randomStartPos, transform.position.y, transform.position.z);

        // Calculate the start time based on the initial position
        startTime = Time.time - Mathf.Abs((transform.position.x - leftBorder) / speed);
    }

    void Update()
    {
        float pingPong = Mathf.PingPong((Time.time - startTime) * speed, journeyLength);
        transform.position = new Vector3(leftBorder + pingPong, transform.position.y, transform.position.z);
    }
}
