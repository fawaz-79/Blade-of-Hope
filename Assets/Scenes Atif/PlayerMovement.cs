using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("إعدادات السرعة والقفز")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("التحقق من الأرضية")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // التحقق مما إذا كان اللاعب يلمس الأرض
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // تثبيت اللاعب على الأرض
        }

        // استقبال المدخلات من أزرار الاتجاهات (WASD أو الأسهم)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // تحريك اللاعب بناءً على الاتجاه الذي ينظر إليه
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // القفز (Space)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // تطبيق الجاذبية
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}