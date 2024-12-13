using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiEidham : MonoBehaviour
{
    public float attackRange = 2.0f; // Distance to start attacking
    public float attackCooldown = 1.0f; // Cooldown time between attacks
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isAttacking = false;
    private float lastAttackTime;
    Animator animator; 
    int enemyLife = 3;
    PlayerControllerEidham playerControllereidham;
    private GameObject player;  // Declare the player variable
    private bool isDead = false; // Track if the enemy is dead
    private bool isStunned = false;
    private Vector3 originalDestination;

    public GameObject[] powerUpPrefabs; // Array of power-up prefabs

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Ensure gravity is enabled
        rb.useGravity = true;
        rb.isKinematic = true; // Make sure Rigidbody does not interfere with NavMeshAgent

        player = GameObject.Find("Player");
        playerControllereidham = player.GetComponent<PlayerControllerEidham>();
    }

    void Update()
    {
        if (isDead) return; // Prevent further actions if the enemy is dead

        if (isStunned) return; // Prevent further actions if the enemy is stunned

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < attackRange)
        {
            // If within attack range, attack the player
            if (!isAttacking && Time.time > lastAttackTime + attackCooldown)
            {
                StartCoroutine(Attack());
            }
        }
        else if (distanceToPlayer < 15)
        {
            // If within chase range but not attack range, chase the player
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(player.transform.position);
                animator.SetInteger("ZombieState", 1);
            }
        }
        else
        {
            // If not within chase range, stop moving
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(transform.position);
                animator.SetInteger("ZombieState", 0);
            }
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        animator.SetTrigger("Attack"); // An attack animation

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(attackCooldown);

        // Implement damage here
        playerControllereidham.TakeDamage(1); // Assuming player has a TakeDamage method

        lastAttackTime = Time.time;

        isAttacking = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (isDead) return; // Ignore further collisions if the enemy is dead

        if (col.gameObject.tag == "BulletLevel3")
        {
            enemyLife--;
            animator.SetTrigger("ZombieGetHit");

            if (enemyLife <= 0 && !isDead)
            {
                isDead = true;
                agent.enabled = false; // Disable the NavMeshAgent
                GetComponent<Collider>().enabled = false; // Disable the Collider
                animator.SetTrigger("ZombieDie");
                playerControllereidham.killZombie++;
                Debug.Log("Easyy!!!!!!!");

                // Spawn a power-up where the enemy died
                SpawnPowerUp(transform.position);

                Destroy(gameObject, 2.7f);
            }
        }
    }

    void SpawnPowerUp(Vector3 position)
    {
        // Check if the powerUpPrefabs array is not empty
        if (powerUpPrefabs.Length > 0)
        {
            // Choose a random power-up prefab
            int prefabIndex = Random.Range(0, powerUpPrefabs.Length);
            GameObject selectedPowerUpPrefab = powerUpPrefabs[prefabIndex];

            // Adjust the spawn position to be higher
            Vector3 spawnPosition = new Vector3(position.x, position.y + 0.5f, position.z);

            // Spawn the selected power-up at the adjusted position
            Instantiate(selectedPowerUpPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No power-up prefabs assigned to powerUpPrefabs array.");
        }
    }

    public void StunEnemy(float stunDuration)
    {
        if (!isStunned && agent.isOnNavMesh)
        {
            isStunned = true;
            agent.isStopped = true; // Stop the enemy's NavMeshAgent
            originalDestination = agent.destination;
            PlayStunAnimation();
            StartCoroutine(EndStunAfterDelay(stunDuration));
        }
    }

    IEnumerator EndStunAfterDelay(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        EndStun();
    }

    public void EndStun()
    {
        if (isStunned)
        {
            isStunned = false;
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false; // Resume the enemy's NavMeshAgent
                agent.SetDestination(originalDestination);
            }
        }
    }

    public void PlayStunAnimation()
    {
        animator.SetTrigger("ZombieStunned"); // Trigger stun animation in Animator
    }
}
