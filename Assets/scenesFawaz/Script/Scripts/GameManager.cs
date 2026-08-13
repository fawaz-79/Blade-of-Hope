using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// يدير حالة اللعبة العامة: النقاط (المسافة المقطوعة)، شاشة الخسارة، وإعادة التشغيل.
/// ضع هذا السكربت على Object فارغ في المشهد اسمه مثلًا "GameManager".
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("مراجع")]
    public PlayerController player;

    [Header("واجهة المستخدم (اختياري - اتركها فارغة إن لم تكن قد أنشأت UI بعد)")]
    public Text scoreText;
    public GameObject gameOverPanel;
    public Text finalScoreText;

    private float distanceTraveled;
    private bool isGameOver;
    private Vector3 playerStartPosition;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

        if (player != null)
        {
            playerStartPosition = player.transform.position;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver || player == null) return;

        // النقاط = المسافة المقطوعة للأمام منذ بداية الجولة
        distanceTraveled = player.transform.position.z - playerStartPosition.z;

        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(distanceTraveled).ToString() + " m";
        }
    }

    /// <summary>
    /// يُستدعى من PlayerController عند الاصطدام بعائق.
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "المسافة: " + Mathf.FloorToInt(distanceTraveled) + " م";
        }

        // إيقاف الزمن اختياري لتجميد الحركة تمامًا عند الخسارة
        Time.timeScale = 0f;
    }

    /// <summary>
    /// أعد ربط هذه الدالة بزر "إعادة المحاولة" في الـ UI.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
