using UnityEditor.EditorTools;
using UnityEngine;

// 해당 클래스는 여러 매니저에 쉽게 접근하기 위한 클래스
public static class CManagers
{
    private static CoinPoolManager _pool;

    public static CoinPoolManager Pool
    {
        get
        {
            if (_pool == null)
            {
                GameObject obj = new GameObject("CoinPoolManager");
                _pool = obj.AddComponent<CoinPoolManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _pool;
        }
    }
}
