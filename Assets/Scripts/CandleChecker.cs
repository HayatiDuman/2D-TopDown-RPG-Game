using UnityEngine;

public class CandleChecker : MonoBehaviour
{
    public Candle candle;
    private bool isCorrect = false;

    private void Update()
    {
        if (candle!= null && candle.isLit)
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
        return isCorrect;
    }
}
