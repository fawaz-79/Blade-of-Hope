using UnityEngine;

/// <summary>
/// يتحكم بحركة اللاعب في لعبة الـ Speed Run.
/// اللاعب يتحرك للأمام دائمًا بسرعة ثابتة، واللاعب البشري
/// يتحكم فقط باللف يمين/يسار للتنقل بين 3 لاينات ثابتة لتجنب العوائق.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("إعدادات السرعة")]
    [Tooltip("سرعة الجري للأمام (ثابتة طوال اللعب)")]
    public float forwardSpeed = 12f;

    [Tooltip("قوة الجاذبية المطبّقة على اللاعب")]
    public float gravity = -20f;

    [Header("إعدادات اللاينات (Lanes)")]
    [Tooltip("عدد اللاينات - يفضل إبقاؤه 3 (يسار - وسط - يمين)")]
    public int laneCount = 3;

    [Tooltip("المسافة بين كل لاين والذي يليه")]
    public float laneDistance = 3f;

    [Tooltip("سرعة الانتقال (اللف) بين اللاينات - كل ما زادت كان الانتقال أسرع/أكثر حدة")]
    public float laneChangeSpeed = 12f;

    [Header("مراجع اختيارية")]
    [Tooltip("مرجع لمدير اللعبة (يُملأ تلقائيًا إذا تُرك فارغًا)")]
    public GameManager gameManager;

    private CharacterController controller;
    private int currentLane;      // اللاين الحالي: 0 = يسار, 1 = وسط, 2 = يمين
    private float targetX;        // الموضع الأفقي المستهدف بناءً على اللاين
    private float verticalVelocity;
    private bool isDead;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // اللاعب يبدأ دائمًا في اللاين الأوسط
        currentLane = laneCount / 2;
        targetX = LaneToX(currentLane);

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    void Update()
    {
        if (isDead) return;

        HandleLaneInput();
        Move();
    }

    /// <summary>
    /// يقرأ ضغطات الأسهم أو A/D ويحرك اللاعب لاين وحدة في كل مرة.
    /// </summary>
    private void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }
    }

    /// <summary>
    /// ينقل اللاعب لاين واحد يسار (-1) أو يمين (+1)، مع منعه من الخروج عن حدود اللاينات.
    /// </summary>
    private void ChangeLane(int direction)
    {
        int newLane = Mathf.Clamp(currentLane + direction, 0, laneCount - 1);
        if (newLane == currentLane) return; // بالفعل في أقصى لاين، لا تفعل شيء

        currentLane = newLane;
        targetX = LaneToX(currentLane);
    }

    /// <summary>
    /// يحوّل رقم اللاين إلى إحداثية X محلية بالنسبة لموضع بداية المسار.
    /// اللاين الأوسط دائمًا عند X = 0.
    /// </summary>
    private float LaneToX(int lane)
    {
        int middle = (laneCount - 1) / 2;
        return (lane - middle) * laneDistance;
    }

    private void Move()
    {
        // الجاذبية (يبقى اللاعب ملتصقًا بالأرض)
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity += gravity * Time.deltaTime;

        // الانتقال السلس (اللف) نحو اللاين المستهدف أفقيًا
        float currentX = transform.position.x;
        float smoothX = Mathf.Lerp(currentX, targetX, laneChangeSpeed * Time.deltaTime);
        float deltaX = smoothX - currentX;

        Vector3 move = new Vector3(deltaX, verticalVelocity * Time.deltaTime, forwardSpeed * Time.deltaTime);
        controller.Move(move);
    }

    /// <summary>
    /// يُستدعى عند الاصطدام بعائق. يوقف اللاعب ويبلّغ مدير اللعبة.
    /// اربط هذه الدالة بسكربت Obstacle.cs الموجود على كل عائق.
    /// </summary>
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        forwardSpeed = 0f;

        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    public bool IsDead => isDead;
}
