using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject coinPrefab;      // اسحب مجسم العملة الجاهز (Prefab) هنا
    public Transform playerTransform;  // اسحب مجسم اللاعب هنا لمعرفة موقعه

    public float laneDistance = 3.0f;  // المسافة بين المسارات (مثل كود التحكم باللاعب)
    public float spawnDistance = 30f;  // كم يبتعد مكان إنشاء العملات أمام اللاعب
    public float spawnInterval = 10f;  // المسافة بين كل مجموعة عملات والأخرى

    private float lastSpawnZ = 0f;     // آخر نقطة Z تم إنشاء عملات عندها

    void Start()
    {
        // إعطاء قيمة أولية لنقطة البداية
        if (playerTransform != null)
        {
            lastSpawnZ = playerTransform.position.z;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // التحقق مما إذا كان اللاعب قد قطع مسافة كافية لإنشاء مجموعة عملات جديدة
        if (playerTransform.position.z + spawnDistance > lastSpawnZ + spawnInterval)
        {
            SpawnCoinGroup();
        }
    }

    void SpawnCoinGroup()
    {
        // 1. اختيار مسار عشوائي: 0 (يسار)، 1 (وسط)، 2 (يمين)
        int randomLane = Random.Range(0, 3);

        // 2. تحويل رقم المسار إلى إحداثيات X (-3، 0، 3)
        float xPosition = (randomLane - 1) * laneDistance;

        // 3. تحديد موقع Z القادم بناءً على آخر نقطة توليد
        float zPosition = lastSpawnZ + spawnInterval;
        lastSpawnZ = zPosition; // تحديث الموقع الأخير

        // 4. إنشاء صف من 4 عملات متتالية خلف بعضها بنفس المسار
        for (int i = 0; i < 4; i++)
        {
            // وضع العملة على ارتفاع 1 متر من الأرض ومتباعدة بـ 2 متر على محور Z
            Vector3 spawnPos = new Vector3(xPosition, 1f, zPosition + (i * 2f));

            // دالة Instantiate تقوم بإنشاء نسخة من العملة في المشهد
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }
}
