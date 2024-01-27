using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingSystem : MonoBehaviour
{
    public GameObject brushPrefab;
    public Camera drawingCamera;
    public Image drawingPanel;

    public LineRenderer currentLine;
    public List<Vector2> pointsList = new List<Vector2>();
    public bool isDrawing = false;
    public bool isDrawn = false;
    public float brushSize;
    public Color brushColor;
    public float brushDistance;

    private RectTransform panelRect;

    void Start()
    {
        panelRect = drawingPanel.rectTransform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDrawn)
        {
            StartDrawing();
        }
        else if (!isDrawn && !isDrawing)
        {
            return;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
        }

        if (isDrawing)
        {
            Draw();
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        GameObject brush = Instantiate(brushPrefab, GetMousePosition(), Quaternion.identity);
        currentLine = brush.GetComponent<LineRenderer>();

        currentLine.material.color = brushColor;
        currentLine.startWidth = brushSize;
        currentLine.endWidth = brushSize;

        pointsList.Clear();
    }

    void StopDrawing()
    {
        isDrawing = false;
        isDrawn = true;
        if (currentLine)
        {
            currentLine.startWidth = brushSize;
            currentLine.endWidth = brushSize;
            currentLine.material.color = brushColor;

            Rigidbody2D rigidbody2D = ConvertToRigidbody2D(currentLine.positionCount, pointsList);
            ApplyPhysics(rigidbody2D);
            Destroy(currentLine.gameObject);
        }
    }

    void Draw()
    {
        Vector2 mousePos = GetMousePosition();

        // Check if the mouse position is within the bounds of the panel
        if (IsMouseWithinPanelBounds(mousePos))
        {
            pointsList.Add(mousePos);
            currentLine.positionCount = pointsList.Count;
            currentLine.SetPosition(pointsList.Count - 1, new Vector3(mousePos.x, mousePos.y, 0));
        }
        else
        {
            // Stop drawing if outside the panel bounds
            StopDrawing();
        }

        // Check the total length of the drawn path
        float totalDistance = 0f;
        for (int i = 1; i < pointsList.Count; i++)
        {
            totalDistance += Vector2.Distance(pointsList[i - 1], pointsList[i]);
        }

        if (totalDistance >= brushDistance)
        {
            StopDrawing();
        }
    }

    Vector2 GetMousePosition()
    {
        Ray ray = drawingCamera.ScreenPointToRay(Input.mousePosition);
        return ray.origin;
    }

    bool IsMouseWithinPanelBounds(Vector2 mousePos)
    {
        // Convert the mouse position to local coordinates of the panel
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRect, mousePos, null, out localMousePos);

        // Check if the local mouse position is within the panel bounds
        return panelRect.rect.Contains(localMousePos);
    }

    public Rigidbody2D ConvertToRigidbody2D(int positionCount, List<Vector2> points)
    {
        GameObject pathObject = new GameObject("PathObject");
        LineRenderer pathRenderer = pathObject.AddComponent<LineRenderer>();
        pathRenderer.positionCount = positionCount;
        Vector3[] convertedPoints = new Vector3[points.Count];

        for (int i = 0; i < points.Count; i++)
        {
            convertedPoints[i] = new Vector3(points[i].x, points[i].y, 0);
        }

        pathRenderer.SetPositions(convertedPoints);

        pathRenderer.startWidth = brushSize;
        pathRenderer.endWidth = brushSize;
        pathRenderer.material.color = brushColor;

        // Add EdgeCollider2D
        EdgeCollider2D edgeCollider = pathObject.AddComponent<EdgeCollider2D>();
        edgeCollider.points = points.ToArray(); // Use the points directly

        Rigidbody2D rigidbody2D = pathObject.AddComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        pathRenderer.useWorldSpace = false;

        return rigidbody2D;
    }


    public void ApplyPhysics(Rigidbody2D rigidbody2D)
    {
        // Add any additional physics-related settings here
    }

    public void Redraw()
    {
        Debug.Log("here");
        // Clear existing drawn points
        pointsList.Clear();

        // Destroy the existing LineRenderer if it exists
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
        }

        // Reset drawing state
        isDrawn = false;
        Destroy(GameObject.Find("PathObject"));

        // Start drawing again if needed
        // StartDrawing(); // Uncomment this line if you want to start drawing immediately after clicking redraw
    }

}
