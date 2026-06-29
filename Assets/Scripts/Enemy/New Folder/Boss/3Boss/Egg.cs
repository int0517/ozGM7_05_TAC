using UnityEngine;

public class Egg : MonoBehaviour
{
    [Header("¾Ë ±âº» ½ºÅÝ")]
    [SerializeField] private float eggMaxHP = 5;
    private float eggcurrentHP;
    [SerializeField] private float spawnInterval = 3;
    [SerializeField] private EnemyHpUI eggUI;
    [SerializeField] protected GameObject oneEnemyPrefab;
    [SerializeField] protected GameObject twoEnemyPrefab;
    private float spawnTimer = 0;
    void Start()
    {
        eggcurrentHP = eggMaxHP;
    }
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
        GameObject prefabToSpawn = (Random.value > 0.5f) ? oneEnemyPrefab : twoEnemyPrefab;

        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }

    public void TakeDamage(float damage)
    {
        eggcurrentHP -= damage;
        eggUI.UpdateHealthBar(eggcurrentHP, eggMaxHP);
        if (eggcurrentHP <= 0)
        {
            Destroy(gameObject);
        }

    }
}
