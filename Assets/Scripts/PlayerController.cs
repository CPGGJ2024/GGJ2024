using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    public float maxHorizVel = 5f;
    private Vector3 velocity = Vector3.zero;
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input handling
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate movement direction
        Vector2 movement = new Vector2(horizontalInput, 0f).normalized;

        if (movement.magnitude > 0.1f && rb.velocity.x > -maxHorizVel &&  rb.velocity.x < maxHorizVel)
        {
            // Calculate and apply the new position
            rb.AddForce(movement * moveSpeed);
        }
    }
}
