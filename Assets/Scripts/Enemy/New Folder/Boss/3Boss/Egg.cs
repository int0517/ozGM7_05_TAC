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

        ShortRangeEnemy shortEnemy = prefabToSpawn.GetComponent<ShortRangeEnemy>();
        if (shortEnemy != null)
        {
            ShortRangeEnemy enemy =
                EManagers.Pool.GetPool(shortEnemy);

            enemy.transform.position = transform.position;
            enemy.transform.rotation = Quaternion.identity;
            enemy.Init();
            return;
        }

        LongRangeEnemy longEnemy = prefabToSpawn.GetComponent<LongRangeEnemy>();
        if (longEnemy != null)
        {
            LongRangeEnemy enemy =
                EManagers.Pool.GetPool(longEnemy);

            enemy.transform.position = transform.position;
            enemy.transform.rotation = Quaternion.identity;
            enemy.Init();
            return;
        }
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
