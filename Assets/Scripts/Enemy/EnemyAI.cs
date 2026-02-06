using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;
    public int attackDamage = 10;

    [Header("Block Movement")]
    public float blockSpeedMultiplier = 0.4f;

    Transform player;
    float lastAttackTime;

    Rigidbody2D rb;
    EnemyHealth enemyHealth;
    EnemyStamina enemyStamina;
    EnemySkill enemySkill; // 🔥 YENİ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyStamina = GetComponent<EnemyStamina>();
        enemySkill = GetComponent<EnemySkill>(); // 🔥 YENİ
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    void MoveTowardsPlayer()
    {
        float currentSpeed = moveSpeed;

        if (enemyHealth != null && enemyHealth.IsBlocking())
        {
            currentSpeed *= blockSpeedMultiplier;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * currentSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemyStamina != null && enemyStamina.IsBlocking())
            {
                Debug.Log("⚠️ Enemy block yapıyor, saldırı yapamıyor!");
                return;
            }

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);

                    // 🔥 Skill puanı kazan
                    if (enemySkill != null)
                    {
                        enemySkill.AddSkillPoints(enemySkill.skillPointsPerHit);
                    }
                }

                lastAttackTime = Time.time;
                Debug.Log("💥 Enemy saldırdı!");
            }
        }
    }
}