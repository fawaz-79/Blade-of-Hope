using UnityEngine;

/// <summary>
/// يوضع هذا السكربت على كل عائق (Obstacle Prefab).
/// عند اصطدام اللاعب به، تُستدعى Die() على PlayerController.
/// تأكد أن الـ Collider على العائق واللاعب مضبوطين بشكل صحيح
/// (راجع ملف README لشرح إعداد الـ Colliders والـ Tags).
/// </summary>
public class Obstacle : MonoBehaviour
{
    [Tooltip("المسافة خلف اللاعب التي يتم بعدها تدمير هذا العائق تلقائيًا لتفريغ الذاكرة")]
    public float destroyBehindOffset = 15f;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        // تدمير العائق تلقائيًا بعد أن يتجاوزه اللاعب بمسافة كافية
        if (player != null && transform.position.z < player.position.z - destroyBehindOffset)
        {
            Destroy(gameObject);
        }
    }

    // إن كان الـ Collider على اللاعب من نوع Trigger (موصى به مع CharacterController)
    void OnTriggerEnter(Collider other)
    {
        HandleHit(other.gameObject);
    }

    // احتياطًا في حال استُخدم Collider عادي بدل Trigger
    void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject);
    }

    private void HandleHit(GameObject other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.Die();
        }
    }
}
