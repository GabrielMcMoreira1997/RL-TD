using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    public float speed = 10f;
    public float knockbackForce = 2f;
    private Transform target;
    private float damage;
    private Tower ownerTower;

    public void Init(Transform target, float damage, Tower tower)
    {
        this.target = target;
        this.damage = damage;
        this.ownerTower = tower;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            Enemie enemie = other.GetComponent<Enemie>();

            if (enemie != null)
            {
                enemie.health -= damage;

                if (enemie.rb != null)
                {
                    Vector2 knockDir = (enemie.transform.position - transform.position).normalized;
                    enemie.ApplyKnockback(knockDir, knockbackForce, 0.2f);

                }

                if (enemie.health <= 0f)
                {
                    ownerTower?.GainXP(enemie.xpValue);
                    Destroy(enemie.gameObject);
                }

                Destroy(gameObject); // destrói o projétil após o impacto
            }
        }
    }
}
