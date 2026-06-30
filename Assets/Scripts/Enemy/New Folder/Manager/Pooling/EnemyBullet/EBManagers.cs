using UnityEditor.EditorTools;
using UnityEngine;

// 해당 클래스는 여러 매니저에 쉽게 접근하기 위한 클래스
public static class EBManagers
{
    private static EnemyBulletPoolManager _pool;

    public static EnemyBulletPoolManager Pool
    {
        get
        {
            if (_pool == null)
            {
                GameObject obj = new GameObject("EnemyBulletPoolManager");
                _pool = obj.AddComponent<EnemyBulletPoolManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _pool;
        }
    }
}
