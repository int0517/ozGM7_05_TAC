using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class pSkill3_Laser : MonoBehaviour
{
    [Header("스킬 레벨")]
    [SerializeField] private int skill3Level = 0;

    [Header("공격 쿨타임")]
    [SerializeField] private float attackTimerMax = 5.0f;
    [SerializeField] private float attackTimer = 0f;
    [SerializeField] private float laserTime = 1f;

    [SerializeField] private GameObject[] lasers;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float searchLength = 8f;
    private Vector2 moveDirection1, moveDirection2, moveDirection3;
    private bool isLaserActive = false;

    void Start()
    {
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].SetActive(false);
        }
    }

    void Update()
    {
        transform.position = playerTransform.position;

        if (attackTimer < attackTimerMax) attackTimer += Time.deltaTime;

        if (!isLaserActive && attackTimer >= attackTimerMax) StartCoroutine(LaserActive());
    }

    private void SearchEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, searchLength, targetLayer);

        moveDirection1 = Vector2.zero;
        moveDirection2 = Vector2.zero;
        moveDirection3 = Vector2.zero;

        if (enemies.Length == 0) return;

        System.Array.Sort(enemies, (a, b) =>
        {
            float distA = (a.transform.position - transform.position).sqrMagnitude;
            float distB = (b.transform.position - transform.position).sqrMagnitude;
            return distA.CompareTo(distB);
        });

        // 가장 가까운 적
        moveDirection1 =
            ((Vector2)enemies[0].transform.position - (Vector2)transform.position).normalized;

        // 적이 2마리 이상일 때 가장 먼 적
        if (enemies.Length >= 2)
        {
            moveDirection2 =
                ((Vector2)enemies[enemies.Length - 1].transform.position - (Vector2)transform.position).normalized;
        }

        // 적이 3마리 이상일 때 중간 적
        if (enemies.Length >= 3)
        {
            int middleIndex = enemies.Length / 2;

            moveDirection3 =
                ((Vector2)enemies[middleIndex].transform.position - (Vector2)transform.position).normalized;
        }
    }

    IEnumerator LaserActive()
    {
        isLaserActive = true;

        SearchEnemy();

        if (moveDirection1 != Vector2.zero)
        {
            lasers[0].SetActive(true);
            lasers[0].transform.right = moveDirection1;
        }

        if (moveDirection2 != Vector2.zero)
        {
            lasers[1].SetActive(true);
            lasers[1].transform.right = moveDirection2;
        }

        if (moveDirection3 != Vector2.zero)
        {
            lasers[2].SetActive(true);
            lasers[2].transform.right = moveDirection3;
        }

        attackTimer = 0;

        yield return new WaitForSeconds(laserTime);

        for (int i = 0; i < lasers.Length; i++)
        {
            if (lasers[i].activeSelf == true) lasers[i].SetActive(false);
        }

        isLaserActive = false;
    }
}
