using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f; 
    
    public float defaultMoveSpeed = 8f;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    public LayerMask solidObjectsLayer; // �arp��ma kontrol� i�in katman
    public LayerMask interactableLayer; // Etkile�im kontrol� i�in katman
    public LayerMask nesneLayer;
    public LayerMask boxLayer;
    public LayerMask enemyLayer;
    private BoxCollider2D boxCollider;

    public Transform tempParent = null;
    public Vector3 facingDir = Vector3.zero;
    public bool kutuPozisyon = false;
    public bool basla = true;

    public Transform holdPoint;
    public float tasimaHizi = 5f;

    private GameObject nesneTut = null;
    private bool mesaleTasiyor = false;
    public BoxChecker[] boxes;
    public GameObject torch;
    public CandleChecker[] candles;
    public SamdanKapi kapi;
    public bool anahtarEldemi = false;

    private bool gorunur = false;


    public static PlayerController Ornek { get; private set; }


    private void Awake()
    {
        holdPoint = GameObject.Find("Character").transform;
        animator = GetComponent<Animator>();
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        BoxColliderAyarla(boxCollider);
    }

    private void BoxColliderAyarla(BoxCollider2D boxCollider)
    {
        // BoxCollider boyutu ve offset ayarlar�
        boxCollider.size = new Vector2(0.5f, 0.7f);
        boxCollider.offset = new Vector2(0f, -0.28f);
    }

    public void Update()
    {
        if (!isMoving)
        {
            input.x = (Input.GetAxisRaw("Horizontal")) / 4;
            input.y = (Input.GetAxisRaw("Vertical")) / 4;

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var hedefPozisyon = (Vector2)transform.position + input;

                if (YuruneBilirmi(hedefPozisyon) && !DialogManager.Ornek.DiyalogKontrol())
                {
                    StartCoroutine(HareketEt(hedefPozisyon));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);

        //Etkile�ime girme
        if (Input.GetKeyDown(KeyCode.Z))
            Etkilesim();

        //Nesne ta��ma
        if (basla && Input.GetKeyDown(KeyCode.E))
        {
            Tut();
            basla = false;
        }
        //Nesne b�rakma
        if (!basla && Input.GetKeyDown(KeyCode.R))
        {
            Birak();
            basla = true;
        }
        //�amdan yakma
        if (mesaleTasiyor && Input.GetKeyDown(KeyCode.F))
        {
            MumYak();
        }
        //Kutu pozisyonlar� do�ruysa me�ale g�r�n�r oldu.
        if (!gorunur && KutuPozisyonuKontrol())
        {
            gorunur = true;
            Debug.Log("Me�ale g�r�n�r oldu.");
            torch.SetActive(true);
        }
        //�amdan odas�n�n kap�s� i�in �amdan kontrolleri
        if (CheckAllCandles())
        {
            kapi.KapiAc();
        }

        DusmanTemas();
    }

    private void FixedUpdate()
    {
        facingDir = new Vector3((animator.GetFloat("moveX")) * 2, (animator.GetFloat("moveY")) * 2);
    }

    private IEnumerator HareketEt(Vector3 hedefPozisyon)
    {
        isMoving = true;

        while ((hedefPozisyon - transform.position).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, hedefPozisyon, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = hedefPozisyon;
        isMoving = false;
    }

    private bool YuruneBilirmi(Vector3 hedefPozisyon)
    {
        Vector3 boyut = boxCollider.size;
        Vector2 offset = boxCollider.offset;
        Vector2 kutuPozisyonu = hedefPozisyon + (Vector3)offset;

        Collider2D collider = Physics2D.OverlapBox(kutuPozisyonu, boyut, 0.2f, solidObjectsLayer | interactableLayer);


        return collider == null;
    }

    private void Etkilesim()
    {
        //facingDir = new Vector3((animator.GetFloat("moveX")) * 2, (animator.GetFloat("moveY")) * 2);
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.5f, interactableLayer);

        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.InterfaceEtkilesim();
        }
    }

    // Nesne alma i�lemleri
    private void Tut()
    {
        var interactPos = transform.position + facingDir;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactPos, 0.5f, interactableLayer | nesneLayer | boxLayer);

        //if (Physics2D.OverlapCircleAll(interactPos, 0.5f, interactableLayer) != null)

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Box"))
            {
                nesneTut = collider.gameObject;
                nesneTut.transform.SetParent(GameObject.Find("Character").transform);
                nesneTut.transform.localPosition = facingDir * 2;
                nesneTut.layer = 12;

                nesneTut.GetComponent<Rigidbody2D>().isKinematic = false;
                moveSpeed = tasimaHizi;
                break;
            }
            else if (collider.CompareTag("Torch"))
            {
                nesneTut = collider.gameObject;
                nesneTut.transform.SetParent(GameObject.Find("Character").transform);
                nesneTut.transform.localPosition = facingDir * 2;

                mesaleTasiyor = true;
                break;
            }
            else if (collider.CompareTag("Key"))
            {
                anahtarEldemi = true;
                nesneTut = collider.gameObject;
                nesneTut.transform.SetParent(GameObject.Find("Character").transform);
                nesneTut.transform.localPosition = facingDir * 2;

                break;
            }
        }
    }
    private void Birak()
    {
        if (nesneTut != null)
        {
            if (nesneTut.CompareTag("Key"))
            {
                anahtarEldemi = false;
                nesneTut.transform.SetParent(GameObject.Find("Chest1").transform);
            }
            else if(nesneTut.CompareTag("Torch"))
            {
                nesneTut.transform.SetParent(GameObject.Find("Platform").transform);
                moveSpeed = defaultMoveSpeed;
                mesaleTasiyor = false;
            }
            else
            {
                nesneTut.transform.SetParent(GameObject.Find("Platform").transform);
                nesneTut.layer = 9;
                moveSpeed = defaultMoveSpeed;
                mesaleTasiyor = false;
            }

        }
    }

    private void MumYak()
    {
        var interactPos = transform.position + facingDir;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactPos, 0.5f, interactableLayer | nesneLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Candle"))
            {
                Candle candle = collider.GetComponent<Candle>();
                if (candle != null && !candle.isLit)
                {
                    candle.LightCandle();
                    break;
                }
            }
        }
    }

    public bool AnahtarEldemi()
    {
        if (anahtarEldemi)
            return true;
        else
            return false;
    }

    private bool KutuPozisyonuKontrol()
    {
        foreach (BoxChecker box in boxes)
        {
            if (!box.GetIsCorrect() && box.CompareTag("Box"))
            {
                //Debug.Log("Bir kutu do�ru pozisyonda de�il.");
                return false;
            }
        }
        Debug.Log("T�m kutular do�ru pozisyonda.");
        return true;
    }
    private bool CheckAllCandles()
    {
        foreach (CandleChecker candle in candles)
        {
            if (!candle.GetIsCorrect())
            {
                return false;
            }
        }
        return true;
    }

    private void DusmanTemas()
    {
        Vector3 boyut = boxCollider.size;
        Vector2 offset = boxCollider.offset;

        Collider2D collider = Physics2D.OverlapBox(transform.position, boyut, 0.2f, enemyLayer);
        if (collider != null)
        {
            Debug.Log("Dusmana yakalandin!");
            //D��mana yakalan�nca gelen aray�z.
            SceneManager.LoadScene("RestartMenu");
        }
    }
}