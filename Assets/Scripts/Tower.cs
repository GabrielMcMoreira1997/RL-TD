using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float health = 100f;
    public float damage = 15f;
    public float attackRange = 5f;
    public float attackCooldown = 1f;
    private float attackTimer = 0f;

    [Header("Level Settings")]
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 50;
    public float xpMultiplier = 1.5f;


    public GameObject projectilePrefab;

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Enemie target = FindNearestEnemyInRange();
            if (target != null)
            {
                Attack(target);
                attackTimer = attackCooldown;
            }
        }
    }

    Enemie FindNearestEnemyInRange()
    {
        Enemie[] enemies = FindObjectsByType<Enemie>(FindObjectsSortMode.None);
        Enemie nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Enemie e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < attackRange && dist < minDistance)
            {
                nearest = e;
                minDistance = dist;
            }
        }

        return nearest;
    }

    void Attack(Enemie enemy)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile2D>().Init(enemy.transform, damage, this);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"Torre ganhou {amount} XP! XP atual: {currentXP}");

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.CeilToInt(xpToNextLevel * xpMultiplier);

        // Melhoria dos atributos da torre
        damage += 5f;
        attackRange += 0.5f;
        attackCooldown *= 0.9f;

        Debug.Log($"Torre subiu para o nível {level}!");
    }
}
