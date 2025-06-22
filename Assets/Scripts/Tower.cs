using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float health = 100f;
    public float damage = 15f;
    public float attackRange = 5f;
    public float attackCooldown = 1f;
    private float attackTimer = 0f;

    [Header("Level Settings")]
    [SerializeField] AnimationCurve experienceCurve;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceFill;
    private int level;
    public int currentXP = 0;

    private int previousLevelsExperience, nextLevelsExperience;


    public GameObject projectilePrefab;

    void Start()
    {
        LevelUp();
    }

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

        if(Input.GetButtonDown("Fire1"))
        {
            GainXP(5); // For testing purposes, gain XP on mouse click
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
        Debug.Log("Gained " + amount + " XP. Current XP: " + currentXP + ", to next level: " + nextLevelsExperience);
        CheckForLevelUp();
        UpdateUI();
    }

    private void CheckForLevelUp()
    {
        if (currentXP >= nextLevelsExperience)
        {
            Debug.Log("Subiu de nivel: " + level);
            level++;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(level); //menos 1 porque o nível atual já foi incrementado
        nextLevelsExperience = (int)experienceCurve.Evaluate(level + 1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        int start = currentXP - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience;

        levelText.text = level.ToString();
        experienceText.text = start + "exp / " + end + "xp";
        experienceFill.fillAmount = (float)start / (float)end;
    }
}
