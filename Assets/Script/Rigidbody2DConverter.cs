using UnityEngine;

public class Rigidbody2DConverter : MonoBehaviour
{
    private DrawingSystem drawingSystem;

    void Start()
    {
        // Get the DrawingSystem component
        drawingSystem = GetComponent<DrawingSystem>();
    }

    public void ConvertToRigidbody2D()
    {
        // Disable further drawing
        drawingSystem.enabled = false;

        // Convert the drawn path to Rigidbody2D
        Rigidbody2D rigidbody2D = drawingSystem.ConvertToRigidbody2D(drawingSystem.currentLine.positionCount, drawingSystem.pointsList);
    }
}
