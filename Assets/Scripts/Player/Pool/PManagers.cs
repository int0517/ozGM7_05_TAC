using UnityEditor.EditorTools;
using UnityEngine;

// 해당 클래스는 여러 매니저에 쉽게 접근하기 위한 클래스
public static class PManagers
{
    private static PlayerPoolManager _pool;

    public static PlayerPoolManager Pool
    {
        get
        {
            if (_pool == null)
            {
                GameObject obj = new GameObject("PoolManager");
                _pool = obj.AddComponent<PlayerPoolManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _pool;
        }
    }
}
