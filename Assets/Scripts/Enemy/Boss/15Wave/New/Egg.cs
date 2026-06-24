using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour
{
    [Header("알 기본 스텟")]
    [SerializeField] private int eggMaxHP = 5;
    private int eggcurrentHP;
    [SerializeField] private float spawnInterval = 3;
    [SerializeField] private EnemyHPUI eggUI;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("아아야");
        if (collision.CompareTag("Skill"))
        {
            Debug.Log("아야");
            TakeDamage();
        }
    }
    
    private void TakeDamage()
    {
        eggcurrentHP--;
        eggUI.UpdateHealthBar(eggcurrentHP, eggMaxHP);
        if (eggcurrentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
