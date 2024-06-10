using UnityEngine;

public class BoxChecker : MonoBehaviour
{
    public Transform correctPosition;
    private bool isCorrect = false;
    private Vector2 boxSize;
    public LayerMask nesneLayer;
    public GameObject box;

    private void Update()
    {
        boxSize = new Vector2(0f, 0f);
        Collider2D collider = Physics2D.OverlapBox(correctPosition.position, boxSize, nesneLayer);
        
        if (Vector2.Distance(box.transform.position, correctPosition.position) < 1f)
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }
    }

    public bool GetIsCorrect()
    {
        if (isCorrect) 
            return true;
        else
            return false;
    }
}
