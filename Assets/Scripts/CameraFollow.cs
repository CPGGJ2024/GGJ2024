using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.1f;
    public Vector3 offset;
    public float deadzoneWidth = 2f;
    public float deadzoneHeight = 1.5f;

    private Vector3 velocity = Vector3.zero;
    void Update()
    {
        // if (player == null)
        // {
        //     // Ensure the player reference is set in the Unity editor
        //     Debug.LogWarning("Player reference not set for CameraFollow script!");
        //     return;
        // }

        // // Get the target position
        // Vector3 targetPosition = player.position + offset;
        // // Get the current camera boundaries
        // Vector3 currentPosition = transform.position;

        // // Calculate the deadzone boundaries
        // float minX = targetPosition.x - deadzoneWidth / 2;
        // float maxX = targetPosition.x + deadzoneWidth / 2;
        // float minY = targetPosition.y - deadzoneHeight / 2;
        // float maxY = targetPosition.y + deadzoneHeight / 2;

        // // Smooth towards target position
        // Vector3 toPosition = currentPosition + (targetPosition - currentPosition) * smoothSpeed;

        // // Ensure the camera stays within the deadzone
        // float newX = Mathf.Clamp(toPosition.x, minX, maxX);
        // float newY = Mathf.Clamp(toPosition.y, minY, maxY);

        // transform.position = new(newX, newY, currentPosition.z);


        Vector3 currentPosition = transform.position;

        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothSpeed);
        transform.position = new(transform.position.x, transform.position.y, currentPosition.z);
    }
}
