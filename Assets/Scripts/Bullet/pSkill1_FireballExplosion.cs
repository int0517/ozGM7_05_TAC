using UnityEngine;

public class pSkill1_FireballExplosion : MonoBehaviour
{
    [SerializeField] private pSkill1_FireballBullet fireballPrefab;

    private void OnParticleSystemStopped()
    {
        Managers.Pool.ReturnPool(fireballPrefab.gameObject, fireballPrefab);
    }
}
