using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class EnemyBulletPoolPreLoader : MonoBehaviour
{
    [SerializeField] private EnemyBullets bulletPrefab;
    [SerializeField] private int bulletCount = 50;

    void Start()
    {
        EBManagers.Pool.PreloadPool(bulletPrefab, bulletCount);
    }
}
