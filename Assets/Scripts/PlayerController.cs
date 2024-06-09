using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Inspector'da ayarlanabilir
    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    public LayerMask solidObjectsLayer; // Çarpýþma kontrolü için katman
    public LayerMask interactableLayer; // Etkileþim kontrolü için katman
    public LayerMask nesneLayer;
    public LayerMask boxLayer;
    private BoxCollider2D boxCollider;

    public Transform tempParent = null;
    public Vector3 facingDir = Vector3.zero;
    public bool kutuPozisyon = false;
    public bool basla = true;

    public Transform holdPoint;
    public float tasimaHizi = 3f;
    public Text keyCollectedText;
    public int toplananAnahtarSayisi = 0;

    private GameObject nesneTut = null;
    private bool mesaleTasiyor = false;
    public BoxChecker[] boxes;
    public GameObject torch;
    public CandleChecker[] candles;
    public SamdanKapi kapi;


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
        // BoxCollider boyutu ve offset ayarlarý
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

        //Etkileþime girme
        if (Input.GetKeyDown(KeyCode.Z))
            Etkilesim();

        // Nesne taþýma
        if (Input.GetKeyDown(KeyCode.E))
        {
            Tut();
            basla = false;
        }
        //Nesen býrakma
        if (Input.GetKeyDown(KeyCode.R))
        {
            Birak();
            basla = true;
        }

        if (mesaleTasiyor && Input.GetKeyDown(KeyCode.F))
        {
            MumYak();
        }

        if (!gorunur && KutuPozisyonuKontrol())
        {
            gorunur = true;
            Debug.Log("Meþale görünür oldu.");
            torch.SetActive(true);
        }

        if (CheckAllCandles())
        {
            kapi.KapiAc();
        }
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

    // Nesne alma iþlemleri
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
                nesneTut.transform.localPosition = facingDir;

                mesaleTasiyor = true;
                break;
            }
            else if (collider.CompareTag("Key"))
            {
                nesneTut = collider.gameObject;
                nesneTut.transform.SetParent(GameObject.Find("Character").transform);
                nesneTut.transform.localPosition = facingDir;

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
                nesneTut.transform.SetParent(GameObject.Find("Chest1").transform);
            }
            else if(nesneTut.CompareTag("Torch"))
            {
                nesneTut.transform.SetParent(GameObject.Find("Platform").transform);
                moveSpeed = 5f;
                mesaleTasiyor = false;
            }
            else
            {
                nesneTut.transform.SetParent(GameObject.Find("Platform").transform);
                nesneTut.layer = 9;
                moveSpeed = 5f;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Torch"))
        {
            AnahtarAl(other.gameObject);
        }
    }
    private void AnahtarAl(GameObject keyObject)
    {
        keyObject.SetActive(false);
        ToplananAnahtarGöster();
    }
    private void ToplananAnahtarGöster()
    {
        toplananAnahtarSayisi++;
        keyCollectedText.text = "Toplanan Anahtar: " + toplananAnahtarSayisi.ToString();
    }
    private bool KutuPozisyonuKontrol()
    {
        foreach (BoxChecker box in boxes)
        {
            if (!box.GetIsCorrect())
            {
                //Debug.Log("Bir kutu doðru pozisyonda deðil.");
                return false;
            }
        }
        Debug.Log("Tüm kutular doðru pozisyonda.");
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
}