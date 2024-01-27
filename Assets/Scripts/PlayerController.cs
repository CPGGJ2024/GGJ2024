using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // Input handling
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f).normalized;

        // Move the player
        MovePlayer(movement);
    }

    void MovePlayer(Vector3 direction)
    {
        // Check if there is any input
        if (direction.magnitude > 0f)
        {
            // Calculate and apply the new position
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
    }
}
