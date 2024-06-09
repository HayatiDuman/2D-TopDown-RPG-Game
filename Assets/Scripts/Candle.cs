using UnityEngine;

public class Candle : MonoBehaviour
{
    public GameObject unlitCandle; // Sönük şamdan GameObject'i
    public GameObject litCandle; // Yanan şamdan GameObject'i

    public bool isLit = false; // Şamdanın yanıp yanmadığını belirten boolean

    void Start()
    {
        UpdateCandleState();
    }

    public void LightCandle()
    {
        if (!isLit)
        {
            isLit = true; // Şamdanın yandığını belirt
            UpdateCandleState();
            Debug.Log("Şamdan yanıyor");
        }
    }

    private void UpdateCandleState()
    {
        if (isLit)
        {
            unlitCandle.SetActive(false);
            litCandle.SetActive(true);
        }
        else
        {
            unlitCandle.SetActive(true);
            litCandle.SetActive(false);
        }
    }
}
