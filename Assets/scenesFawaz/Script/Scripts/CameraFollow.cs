using UnityEngine;

/// <summary>
/// كاميرا تتبع اللاعب من الخلف وبزاوية مرتفعة قليلًا (بأسلوب Subway Surfers).
/// تتبع حركة اللف يمين/يسار بسلاسة دون أن تكون ملتصقة تمامًا (لإحساس أفضل بالسرعة).
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("الهدف")]
    public Transform target; // اللاعب

    [Header("الإزاحة عن اللاعب (خلف وأعلى)")]
    public Vector3 offset = new Vector3(0f, 4.5f, -7f);

    [Header("النعومة")]
    [Tooltip("سرعة متابعة الكاميرا للاعب أفقيًا وعموديًا")]
    public float followSmoothTime = 0.12f;

    [Tooltip("هل تتبع الكاميرا حركة اللف يمين/يسار أيضًا؟ (أوصى بتفعيلها لإحساس أفضل)")]
    public bool followLaneMovement = true;

    private Vector3 velocity;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        if (!followLaneMovement)
        {
            // إبقاء الكاميرا مركزة أفقيًا بغض النظر عن اللاين الحالي
            desiredPosition.x = offset.x;
        }

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followSmoothTime);

        // النظر دائمًا نحو نقطة أمام اللاعب قليلًا لإبقاء التركيز على المسار القادم
        Vector3 lookTarget = target.position + Vector3.forward * 5f + Vector3.up * 1f;
        transform.LookAt(lookTarget);
    }
}
