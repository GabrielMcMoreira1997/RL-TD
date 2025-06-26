using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float knockbackForce = 2f;
    private Transform target;
    private float damage;

    public void Init(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

void Update()
{
    if (target == null)
    {
        Destroy(gameObject);
        return;
    }

    Vector2 direction = (target.position - transform.position).normalized;
    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile collided with: " + other.name);
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Projectile hit an enemy: " + other.name);
            Enemie enemie = other.GetComponent<Enemie>();

            if (enemie != null)
            {
                enemie.TakeDamage(damage);

                if (enemie.rb != null)
                {
                    Vector2 knockDir = (enemie.transform.position - transform.position).normalized;
                    enemie.ApplyKnockback(knockDir, knockbackForce, 0.2f);

                }

                Destroy(gameObject); // destrói o projétil após o impacto
            }
        }
    }
}
