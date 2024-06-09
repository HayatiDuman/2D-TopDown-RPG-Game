using UnityEngine;

public class CandleController : MonoBehaviour
{
    public CandleChecker[] candles;

    public static CandleController Ornek { get; private set; }

    public bool CheckAllCandles()
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


