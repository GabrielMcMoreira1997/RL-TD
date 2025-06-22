using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    public float speed = 10f;
    public float knockbackForce = 5f;
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

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            Enemie enemie = target.GetComponent<Enemie>();
            if (enemie != null)
            {
                enemie.health -= damage;

                if (enemie.rb != null)
                {
                    Vector2 knockDir = (target.position - transform.position).normalized;
                    enemie.rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
                }

                if (enemie.health <= 0f)
                {
                    ownerTower?.GainXP(enemie.xpValue);
                    Destroy(enemie.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}
