using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.1f;
    public Vector3 offset;
    public float deadzoneWidth = 2f;
    public float deadzoneHeight = 1.5f;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        FindPathObject();
    }

    void FindPathObject()
    {
        // Attempt to find the PathObject in the scene
        GameObject pathObject = GameObject.Find("PathObject");

        if (pathObject != null)
        {
            player = pathObject.transform;
        }
    }

    void Update()
    {
        if (!player)
        {
            FindPathObject(); // Attempt to find the PathObject if not assigned
            return;
        }
        else
        {
            Vector3 currentPosition = transform.position;

            // Follow the player (PathObject) with smoothing
            transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, smoothSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, currentPosition.z);
        }
    }
}
