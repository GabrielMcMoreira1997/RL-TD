using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject towerPrefab;
    public GameObject[] spawnPoint;
    public float spawnInterval = 2f;

    public void Update()
    {
        spawnInterval -= Time.deltaTime;
        if (spawnInterval <= 0f)
        {
            SpawnEnemy();
            spawnInterval = 2f; // Reset the spawn interval
        }
    }

    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoint.Length);
        GameObject spawnLocation = spawnPoint[randomIndex];
        GameObject enemy = Instantiate(enemyPrefab, spawnLocation.transform.position, Quaternion.identity);
        
        Tower targetTower = FindFirstObjectByType<Tower>();
        
        if (targetTower != null)
        {
            enemy.GetComponent<Enemie>().setTargetTower(targetTower);
        }
    }
}
