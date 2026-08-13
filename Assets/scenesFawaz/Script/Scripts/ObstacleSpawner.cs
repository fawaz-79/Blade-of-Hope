using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// يولّد صفوف من العوائق بشكل دوري أمام اللاعب، موزعة على اللاينات الثلاثة.
/// يضمن دائمًا ترك لاين واحد على الأقل مفتوحًا حتى تبقى اللعبة قابلة للعب دائمًا.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [Header("مراجع أساسية")]
    [Tooltip("مرجع اللاعب - يُستخدم لمعرفة موضعه وسرعته")]
    public PlayerController player;

    [Tooltip("قائمة بريفابات العوائق الممكن توليدها (اختر واحد عشوائيًا لكل موضع)")]
    public GameObject[] obstaclePrefabs;

    [Header("إعدادات التوليد")]
    [Tooltip("المسافة الأمامية (على المحور Z) بين كل صف عوائق والذي يليه")]
    public float rowSpacing = 15f;

    [Tooltip("أقصى عدد لاينات يتم إغلاقها في نفس الصف (يجب أن يكون أقل من عدد اللاينات لضمان وجود ممر مفتوح دائمًا)")]
    public int maxBlockedLanesPerRow = 1;

    [Tooltip("ارتفاع الـ Y الذي توضع عنده العوائق (عادة نفس ارتفاع الأرض)")]
    public float spawnHeight = 0f;

    [Tooltip("مسافة أولى قبل بدء توليد أول صف عوائق (لإعطاء اللاعب وقت استعداد)")]
    public float initialSafeDistance = 20f;

    private float nextSpawnZ;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

        nextSpawnZ = (player != null ? player.transform.position.z : 0f) + initialSafeDistance;
    }

    void Update()
    {
        if (player == null || player.IsDead) return;

        // كلما اقترب اللاعب من نقطة التوليد التالية، ولّد صف عوائق جديد وحدّث النقطة التالية
        while (player.transform.position.z + rowSpacing > nextSpawnZ)
        {
            SpawnRow(nextSpawnZ);
            nextSpawnZ += rowSpacing;
        }
    }

    private void SpawnRow(float zPosition)
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        int laneCount = player.laneCount;

        // اختيار عدد وأماكن اللاينات المُغلقة في هذا الصف، مع ترك لاين واحد مفتوح دائمًا
        int blockedCount = Mathf.Clamp(maxBlockedLanesPerRow, 1, laneCount - 1);

        List<int> lanes = new List<int>();
        for (int i = 0; i < laneCount; i++) lanes.Add(i);
        Shuffle(lanes);

        for (int i = 0; i < blockedCount; i++)
        {
            int lane = lanes[i];
            SpawnObstacleAt(lane, zPosition);
        }
    }

    private void SpawnObstacleAt(int lane, float zPosition)
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        int middle = (player.laneCount - 1) / 2;
        float x = (lane - middle) * player.laneDistance;

        Vector3 spawnPos = new Vector3(x, spawnHeight, zPosition);
        Instantiate(prefab, spawnPos, prefab.transform.rotation);
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
