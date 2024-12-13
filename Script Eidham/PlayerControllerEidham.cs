using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for the UI components
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerControllerEidham : MonoBehaviour
{
    public float moveSpeed = 2;
    public float runSpeed = 4; // Speed when running
    public int killZombie = 0;
    [HideInInspector] public Vector3 dir; 
    float hzInput, vInput;
    CharacterController controller; 

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos; 
  
    [SerializeField] float gravity = -9.01f; 
    Vector3 velocity;

    // Shooting variables
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;

    // Sound Effect
    [SerializeField] private AudioSource GunShotSoundEffect;

    // Health variables
    public int maxHealth = 100;
    private int currentHealth;

    private bool playerisAlive = true; 

    // Health slider
    public Slider healthSlider; // Reference to the UI Slider

    public Animator animator;

    public TextMeshProUGUI killZombieText;
    public TextMeshProUGUI killZombieText2;
    public GameObject GameOverMenu; //Game over Panel


    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth; // Initialize health
        healthSlider.maxValue = maxHealth; // Set the slider's max value
        healthSlider.value = currentHealth; // Initialize the slider's value
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();
        Shoot();

        killZombieText.text = "ZOMBIE KILLED: "+killZombie;
        killZombieText2.text = "ZOMBIE KILLED: "+killZombie;
    }

    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        if(hzInput != 0 || vInput != 0)
        {
            dir = transform.forward * vInput + transform.right * hzInput;

            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isRunning ? runSpeed : moveSpeed;

            // Set the playerState based on whether the player is running or walking
            animator.SetInteger("playerState", isRunning ? 2 : 1);

            controller.Move(dir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetInteger("playerState", 0);
        }
    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        return Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask); 
    }

    void Gravity()
    {
        if (!IsGrounded()) 
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0) 
        {
            velocity.y = -2;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void Shoot()
    {
        if (playerisAlive && Input.GetMouseButtonDown(0)) // Left mouse button
        {
            GunShotSoundEffect.Play();
            animator.SetTrigger("isShooting");
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
         if (!playerisAlive) return;
        
        currentHealth -= damage;
        Debug.Log("Player took damage: " + damage + ", current health: " + currentHealth);

        animator.SetTrigger("getHit");
        healthSlider.value = currentHealth; // Update the slider's value

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerisAlive = false; // Disable Shooting

        Debug.Log("Player died!");
        animator.SetTrigger("playerIsDead");

        // Disable player movement
        controller.enabled = false;

        // Wait for the death animation to finish before showing game over menu
        StartCoroutine(ShowGameOverMenuAfterAnimation());
    }

    IEnumerator ShowGameOverMenuAfterAnimation()
    {
        // Wait for the length of the 'playerIsDead' animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Show the game over menu
        GameOverMenu.SetActive(true);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Check if the controller is initialized before using it
        if (controller != null)
        {
            Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
        }
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthSlider.value = currentHealth; // Update the slider's value
    }
}
