using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class EnemyPoolPreLoader : MonoBehaviour
{
    [Header("근거리")]
    [SerializeField] private ShortRangeEnemy shortEnemy;

    [Header("원거리")]
    [SerializeField] private LongRangeEnemy longEnemy;

    [SerializeField] private int shortCount = 50;
    [SerializeField] private int longCount = 50;

    void Start()
    {
        EManagers.Pool.PreloadPool(shortEnemy, shortCount);
        EManagers.Pool.PreloadPool(longEnemy, longCount);
    }
}
