using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerPoolPreLoader : MonoBehaviour
{
    [Header("프리팹")]
    [SerializeField] private PlayerNormalAttackBullet normalBulletPrefab;
    [SerializeField] private pSkill1_FireballBullet fireballBullet1Prefab;
    [SerializeField] private pSkill1_FireballBullet fireballBullet2Prefab;
    [SerializeField] private pSkill1_FireballBullet fireballBullet3Prefab;
    [SerializeField] private pSkill5_AcidRainController acidrainBulletPrefab;

    [Header("미리 생성할 개수")]
    [SerializeField] private int normalBulletCount = 50;
    [SerializeField] private int fireballBulletCount = 20;
    [SerializeField] private int acidrainBulletCount = 10;

    void Start()
    {
        PManagers.Pool.PreloadPool(normalBulletPrefab, normalBulletCount);
        PManagers.Pool.PreloadPool(fireballBullet1Prefab, fireballBulletCount);
        PManagers.Pool.PreloadPool(fireballBullet2Prefab, fireballBulletCount);
        PManagers.Pool.PreloadPool(fireballBullet3Prefab, fireballBulletCount);
        PManagers.Pool.PreloadPool(acidrainBulletPrefab, acidrainBulletCount);
    }
}
