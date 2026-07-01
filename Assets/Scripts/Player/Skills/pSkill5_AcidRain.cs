using System.Collections;
using UnityEngine;

public class pSkill5_AcidRain : MonoBehaviour
{
    [Header("스킬 레벨")]
    [SerializeField] private int skill5Level = 0;
    public int Skill5Level { get { return skill5Level; } }

    [Header("공격 쿨타임")]
    [SerializeField] private float attackTimerMax = 8.0f;
    [SerializeField] private float attackTimer = 0f;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private pSkill5_AcidRainController acidrainPrefab;
    [SerializeField] float searchLength = 10f;

    private Vector3 targetPosition1, targetPosition2, targetPosition3;

    [SerializeField] private PlayerStat pStat;

    void Update()
    {
        if (!pStat.IsDead)
        {
            if (attackTimer < attackTimerMax) attackTimer +=
                Time.deltaTime * PlayerStatDictionary.PlayerAttackSpeed[pStat.GetStatLvl(PlayerStatEnum.AttackSpeed)];

            if (attackTimer >= attackTimerMax) SpawnAcidRain(skill5Level);
        }
    }


    private void SpawnAcidRain(int skillLvl)
    {
        SearchEnemy();

        switch(skillLvl)
        {
            case 0:
                break;
            case 1:
                pSkill5_AcidRainController bullet1 = Managers.Pool.GetPool(acidrainPrefab);
                bullet1.transform.position = targetPosition1;
                bullet1.transform.rotation = Quaternion.identity;
                bullet1.Init(pStat);
                break;
            case 2:
                pSkill5_AcidRainController bullet2_1 = Managers.Pool.GetPool(acidrainPrefab);
                bullet2_1.transform.position = targetPosition1;
                bullet2_1.transform.rotation = Quaternion.identity;
                bullet2_1.Init(pStat);
                pSkill5_AcidRainController bullet2_2 = Managers.Pool.GetPool(acidrainPrefab);
                bullet2_2.transform.position = targetPosition2;
                bullet2_2.transform.rotation = Quaternion.identity;
                bullet2_2.Init(pStat);
                break;
            case 3:
                pSkill5_AcidRainController bullet3_1 = Managers.Pool.GetPool(acidrainPrefab);
                bullet3_1.transform.position = targetPosition1;
                bullet3_1.transform.rotation = Quaternion.identity;
                bullet3_1.Init(pStat);
                pSkill5_AcidRainController bullet3_2 = Managers.Pool.GetPool(acidrainPrefab);
                bullet3_2.transform.position = targetPosition2;
                bullet3_2.transform.rotation = Quaternion.identity;
                bullet3_2.Init(pStat);
                pSkill5_AcidRainController bullet3_3 = Managers.Pool.GetPool(acidrainPrefab);
                bullet3_3.transform.position = targetPosition3;
                bullet3_3.transform.rotation = Quaternion.identity;
                bullet3_3.Init(pStat);
                break;
            default:
                break;
        }

        attackTimer = 0f;
    }

    private void SearchEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, searchLength, targetLayer);

        targetPosition1 = Vector3.zero;
        targetPosition2 = Vector3.zero;
        targetPosition3 = Vector3.zero;

        if (enemies.Length == 0) return;

        System.Array.Sort(enemies, (a, b) =>
        {
            float distA = (a.transform.position - transform.position).sqrMagnitude;
            float distB = (b.transform.position - transform.position).sqrMagnitude;
            return distA.CompareTo(distB);
        });

        // 가장 가까운 적 위치
        targetPosition1 = enemies[0].transform.position;

        // 적이 2마리 이상일 때 가장 먼 적 위치
        if (enemies.Length >= 2)
        {
            targetPosition2 = enemies[enemies.Length - 1].transform.position;
        }

        // 적이 3마리 이상일 때 중간 적 위치
        if (enemies.Length >= 3)
        {
            int middleIndex = enemies.Length / 2;
            targetPosition3 = enemies[middleIndex].transform.position;
        }
    }

    public void Skill5LevelUp()
    {
        if (skill5Level >= 3) return;

        skill5Level++;
    }
}
