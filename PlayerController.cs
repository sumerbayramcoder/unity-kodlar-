using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    private int score = 0;

    private Animator anim;
    private CharacterController controller;

    void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        UpdateScoreUI();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A (-1) / D (+1)
        float v = Input.GetAxisRaw("Vertical");   // W (+1) / S (-1)

        Vector3 dir = new Vector3(h, 0f, v).normalized;

        bool isWalking = dir.sqrMagnitude > 0.01f;
        if (anim) anim.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            transform.forward = dir; // Karakter yönünü hareket yönüne çevir
            Vector3 move = dir * moveSpeed * Time.deltaTime;

            // CharacterController varsa onu kullan, yoksa transform.position
            if (controller != null)
                controller.Move(move);
            else
                transform.position += move;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger'a girildi: " + other.name + " / Tag: " + other.tag);

        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            score++;
            UpdateScoreUI();
            Debug.Log("Coin toplandı! Yeni skor: " + score);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
