using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerPoolPreLoader : MonoBehaviour
{
    [Header("플레이어 투사체 프리팹")]
    [SerializeField] private PlayerNormalAttackBullet normalBulletPrefab;
    [SerializeField] private pSkill1_FireballBullet fireballBullet1Prefab;
    [SerializeField] private pSkill1_FireballBullet fireballBullet2Prefab;
    [SerializeField] private pSkill1_FireballBullet fireballBullet3Prefab;
    [SerializeField] private pSkill5_AcidRainController acidrainBulletPrefab;

    [Header("적 프리팹")]

    [Header("적 투사체 프리팹")]

    [Header("코인 프리팹")]



    [Header("미리 생성할 개수")]
    [SerializeField] private int normalBulletCount = 50;
    [SerializeField] private int fireballBulletCount = 20;
    [SerializeField] private int acidrainBulletCount = 6;

    void Start()
    {
        Managers.Pool.PreloadPool(normalBulletPrefab, normalBulletCount);
        Managers.Pool.PreloadPool(fireballBullet1Prefab, fireballBulletCount);
        Managers.Pool.PreloadPool(fireballBullet2Prefab, fireballBulletCount);
        Managers.Pool.PreloadPool(fireballBullet3Prefab, fireballBulletCount);
        Managers.Pool.PreloadPool(acidrainBulletPrefab, acidrainBulletCount);
    }
}
