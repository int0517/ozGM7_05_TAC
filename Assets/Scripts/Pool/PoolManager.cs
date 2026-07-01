using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹(GameObject)별 풀
    private Dictionary<GameObject, Queue<Component>> poolDictionary = new();

    // 프리팹(GameObject)별 부모
    private Dictionary<GameObject, Transform> poolParents = new();

    // 모든 풀을 담을 최상위 부모
    private Transform poolRoot;

    private void Awake()
    {
        CreatePoolRoot();
    }

    private void CreatePoolRoot()
    {
        GameObject rootObj = new GameObject("PoolRoot");
        rootObj.transform.SetParent(transform);
        poolRoot = rootObj.transform;
    }

    public void PreloadPool<T>(T prefab, int count) where T : Component
    {
        GameObject key = prefab.gameObject;

        CreatePool(key);

        for (int i = 0; i < count; i++)
        {
            T obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(poolParents[key]);

            poolDictionary[key].Enqueue(obj);
        }
    }

    public T GetPool<T>(T prefab) where T : Component
    {
        GameObject key = prefab.gameObject;

        CreatePool(key);

        T obj;

        if (poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue() as T;
        }
        else
        {
            obj = Instantiate(prefab);
            obj.transform.SetParent(poolParents[key]);
        }

        obj.gameObject.SetActive(true);

        return obj;
    }

    public void ReturnPool<T>(GameObject prefab, T obj) where T : Component
    {
        GameObject key = prefab.gameObject;

        CreatePool(key);

        obj.gameObject.SetActive(false);

        if (obj.transform.parent != poolParents[key])
        {
            obj.transform.SetParent(poolParents[key]);
        }

        poolDictionary[key].Enqueue(obj);
    }

    private void CreatePool(GameObject key)
    {
        if (poolDictionary.ContainsKey(key))
            return;

        poolDictionary.Add(key, new Queue<Component>());

        CreatePoolParent(key);
    }

    private void CreatePoolParent(GameObject key)
    {
        GameObject parentObj = new GameObject(key.name + "_Pool");
        parentObj.transform.SetParent(poolRoot);

        poolParents.Add(key, parentObj.transform);
    }
}