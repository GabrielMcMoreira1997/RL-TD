using UnityEngine;

public class Enemie : MonoBehaviour
{
    [Header("Enemie Default Settings")]
    public float speed = 2f;
    public float health = 10f;
    public float damage = 10f;
    public Tower targetTower;
    public int xpValue = 10;

    public float spawnInterval = 0.6f;
    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void setTargetTower(Tower tower)
    {
        targetTower = tower;
    }
    public void Update()
    {
        if (targetTower != null)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (targetTower.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetTower.transform.position) < 0.5f)
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
}
