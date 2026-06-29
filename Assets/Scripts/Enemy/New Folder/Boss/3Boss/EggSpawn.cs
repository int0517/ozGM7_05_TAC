using UnityEngine;

public class EggSpawn : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 10;
    private float spawnTimer = 0;
    [SerializeField] protected GameObject egg;
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0;
        }
    }
    private void SpawnEnemy()
    {
        Instantiate(egg, transform.position, Quaternion.identity);
    }
}
