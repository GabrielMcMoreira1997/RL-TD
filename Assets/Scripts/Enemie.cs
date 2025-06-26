using UnityEngine;

public class Enemie : MonoBehaviour
{
    [Header("Enemie Default Settings")]
    public float speed = 5f;
    public float maxhealth = 10f;

    public float health = 10f;
    public float damage = 10f;
    public Tower targetTower;
    public int xpValue = 10;

    public float spawnInterval = 0.6f;
    public Rigidbody2D rb;

    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    
    public GameObject healthBarPrefab;
    private HealthBar healthBar;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        health = maxhealth; // Inicializa a saúde com o valor máximo

        GameObject bar = Instantiate(healthBarPrefab);
        healthBar = bar.GetComponent<HealthBar>();
        healthBar.target = this.transform;
        healthBar.setOffset(new Vector3(0.1f, 0.5f, 0)); // Ajuste para cima da cabeça
    }

    public void setTargetTower(Tower tower)
    {
        targetTower = tower;
    }

    public void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
                rb.linearVelocity = Vector2.zero;
            }

            return; // Ignora movimento durante knockback
        }

        if (targetTower != null)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (targetTower.transform.position - transform.position).normalized;
        Vector2 newPosition = rb.position + direction * speed * Time.deltaTime;
        rb.MovePosition(newPosition);

        if (Vector2.Distance(transform.position, targetTower.transform.position) < 0.5f)
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        if (targetTower != null)
        {
            targetTower.health -= damage;
            Debug.Log("Attacked tower! Tower health: " + targetTower.health);

            if (targetTower.health <= 0f)
            {
                Destroy(targetTower.gameObject);
                Debug.Log("Tower destroyed!");
            }
        }
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        isKnockedBack = true;
        knockbackTimer = duration;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxhealth);
        healthBar.SetHealth(health, maxhealth);

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (targetTower != null)
        {
            targetTower.GainXP(xpValue); // Adiciona XP à torre
        }
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }
}
