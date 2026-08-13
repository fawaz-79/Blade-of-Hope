using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int totalCoins = 0;
    public TextMeshProUGUI coinText; // اسحب نص الـ UI هنا (اختياري)

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        Debug.Log("العملات الحالية: " + totalCoins);

        if (coinText != null)
        {
            coinText.text = "Coins: " + totalCoins;
        }
    }
}
