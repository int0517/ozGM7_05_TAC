using UnityEditor.EditorTools;
using UnityEngine;

// 해당 클래스는 여러 매니저에 쉽게 접근하기 위한 클래스
public static class EManagers
{
    private static EnemyPoolManager _pool;

    public static EnemyPoolManager Pool
    {
        get
        {
            if (_pool == null)
            {
                GameObject obj = new GameObject("EnemyPoolManager");
                _pool = obj.AddComponent<EnemyPoolManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _pool;
        }
    }
}
