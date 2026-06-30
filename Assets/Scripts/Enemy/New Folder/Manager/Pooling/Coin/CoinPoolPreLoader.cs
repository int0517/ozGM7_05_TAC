using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class CoinPoolPreLoader : MonoBehaviour
{
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private int coinCount = 300;

    void Start()
    {
        CManagers.Pool.PreloadPool(coinPrefab, coinCount);
    }
}
