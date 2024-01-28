using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject editor;

    private RectTransform editorRectTransform;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        editorRectTransform = editor.GetComponent<RectTransform>();
        initialPosition = editorRectTransform.anchoredPosition;
        StartCoroutine(SlideInEditor());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SlideInEditor()
    {
        float slideDuration = 0.3f; // Adjust the duration as needed

        // Store the initial position
        Vector3 start = editorRectTransform.anchoredPosition;
        Vector3 end = new Vector3(0f, 0f, start.z); // Slide to the middle of the screen

        float startTime = Time.time;

        while (Time.time - startTime < slideDuration)
        {
            float t = (Time.time - startTime) / slideDuration;
            editorRectTransform.anchoredPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }

        editorRectTransform.anchoredPosition = end;

        // You can add additional logic here after the sliding in is complete
    }

    public void OnStartButtonClicked()
    {
        StartCoroutine(SlideOutEditor());
    }

    IEnumerator SlideOutEditor()
    {
        float slideDuration = 0.3f; // Adjust the duration as needed

        // Store the initial position
        Vector3 start = editorRectTransform.anchoredPosition;
        Vector3 end = initialPosition; // Slide back to the initial position

        float startTime = Time.time;

        while (Time.time - startTime < slideDuration)
        {
            float t = (Time.time - startTime) / slideDuration;
            editorRectTransform.anchoredPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }

        editorRectTransform.anchoredPosition = end;

        // You can add additional logic here after the sliding out is complete
    }
}
