using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float deadzoneWidth = 2f;
    public float deadzoneHeight = 1.5f;

    void LateUpdate()
    {
        if (player == null)
        {
            // Ensure the player reference is set in the Unity editor
            Debug.LogWarning("Player reference not set for CameraFollow script!");
            return;
        }

        // Get the target position
        Vector3 targetPosition = player.position + offset;

        // Calculate the deadzone boundaries
        float minX = targetPosition.x - deadzoneWidth / 2;
        float maxX = targetPosition.x + deadzoneWidth / 2;
        float minY = targetPosition.y - deadzoneHeight / 2;
        float maxY = targetPosition.y + deadzoneHeight / 2;

        // Get the current camera position
        Vector3 currentPosition = transform.position;

        // Ensure the camera stays within the deadzone
        float newX = Mathf.Clamp(currentPosition.x, minX, maxX);
        float newY = Mathf.Clamp(currentPosition.y, minY, maxY);

        // Set the target position within the deadzone
        Vector3 smoothedPosition = new Vector3(newX, newY, currentPosition.z);

        // Use Lerp to smoothly move towards the target position
        transform.position = Vector3.Lerp(currentPosition, smoothedPosition, smoothSpeed);
    }
}
