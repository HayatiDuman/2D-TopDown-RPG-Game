using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Vector2 point;
    [SerializeField] public bool sag;
    private Vector2 tempPoint;
    public GameObject dusman;
    SpriteRenderer playerRenderer;

    private void Start()
    {
        playerRenderer = dusman.GetComponent<SpriteRenderer>();
        StartCoroutine(YerDegistir());
    }

    public IEnumerator YerDegistir()
    {
        tempPoint = dusman.transform.position;
        while (playerRenderer != null)
        {
            if (sag)
            {
                sag = !sag;
                playerRenderer.flipX = !playerRenderer.flipX;
                // Hedef pozisyona gider
                while ((Vector2)dusman.transform.position != point)
                {
                    transform.position = Vector2.MoveTowards(transform.position, point, 4f * Time.deltaTime);
                    yield return null; // Frame bitene kadar bekler
                }
                yield return new WaitForSeconds(1f);
            }
            else
            {
                sag = !sag;
                playerRenderer.flipX = !playerRenderer.flipX;
                // Orjinal posizyona gider
                while ((Vector2)dusman.transform.position != tempPoint)
                {
                    transform.position = Vector2.MoveTowards(dusman.transform.position, tempPoint, 4f * Time.deltaTime);
                    yield return null; // Bir sonraki frame için bekler
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}