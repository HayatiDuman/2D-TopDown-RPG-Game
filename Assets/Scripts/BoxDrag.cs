using UnityEngine;

public class BoxDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 originalPosition;

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x, mousePosition.y);
            ToggleDrag();
        }

    }

    public void ToggleDrag()
    {
        isDragging = !isDragging;
        if (isDragging)
        {
            originalPosition = transform.position;
        }
    }
}
