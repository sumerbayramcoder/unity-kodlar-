using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float rotationSpeed = 10f; // Dönüş hızı
    
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
        // Input al (WASD veya Ok tuşları)
        float h = Input.GetAxisRaw("Horizontal"); // A/D veya Sol/Sağ ok
        float v = Input.GetAxisRaw("Vertical");   // W/S veya Yukarı/Aşağı ok
        
        // Hareket vektörü oluştur
        Vector3 dir = new Vector3(h, 0f, v).normalized;
        
        // Hareket var mı kontrol et
        bool isWalking = dir.sqrMagnitude > 0.01f;
        
        // Animator'ı güncelle
        if (anim != null)
        {
            anim.SetBool("isWalking", isWalking);
        }
        
        // Eğer hareket varsa
        if (isWalking)
        {
            // Karakteri yumuşak bir şekilde hareket yönüne çevir
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            // Hareketi uygula
            Vector3 move = dir * moveSpeed * Time.deltaTime;
            
            if (controller != null)
            {
                controller.Move(move);
            }
            else
            {
                transform.position += move;
            }
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
