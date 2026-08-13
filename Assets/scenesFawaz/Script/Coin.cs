using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 100f; // سرعة دوران العملة
    public int coinValue = 1;       // قيمة العملة

    void Update()
    {
        // جعل العملة تدور حول نفسها لمظهر بصري جميل
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // التحقق مما إذا كان الكائن الذي لمس العملة يحمل Tag باسم Player
        if (other.CompareTag("Player"))
        {
            // إضافة العملة إلى المجموع
            if (CoinManager.instance != null)
            {
                CoinManager.instance.AddCoin(coinValue);
            }

            // اختفاء العملة من المشهد
            Destroy(gameObject);
        }
    }
}
