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
    
    private int level, currentXP;

    private int previousLevelsExperience, nextLevelsExperience;

    public GameObject projectilePrefab;

    void Start()
    {
        level = 1; // Começa no nível 1
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

        if (Input.GetButtonDown("Fire1"))
        {
            GainXP(5); // Teste de XP manual
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
        CheckForLevelUp();
        UpdateUI();
    }

    private void CheckForLevelUp()
    {
        // Proteção: só sobe de nível se o próximo XP for válido (> atual)
        if (currentXP >= nextLevelsExperience && nextLevelsExperience > previousLevelsExperience)
        {
            level++;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        float prev = experienceCurve.Evaluate(level);
        float next = experienceCurve.Evaluate(level + 1);

        Debug.Log($"[LevelUp] Level: {level}, PrevXP: {prev}, NextXP: {next}");

        previousLevelsExperience = (int)prev;
        nextLevelsExperience = (int)next;

        UpdateUI();
    }

    private void UpdateUI()
    {
        int start = currentXP - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience;

        Debug.Log($"[UpdateUI] Next: {nextLevelsExperience} | Previous: {previousLevelsExperience} | Start: {start} | End: {end}");

        levelText.text = level.ToString();
        experienceText.text = start + " exp / " + end + " exp";
        experienceFill.fillAmount = end > 0 ? (float)start / end : 0;
    }
}
