using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinPoolManager : MonoBehaviour
{
    private Dictionary<Type, Queue<Component>> poolDictionary = new Dictionary<Type, Queue<Component>>();
    // 타입별 부모 오브젝트용
    private Dictionary<Type, Transform> poolParents = new Dictionary<Type, Transform>();
    // 모든 풀링 오브젝트를 정리할 최상위 부모
    private Transform poolRoot;

    private void Awake()
    {
        CreatePoolRoot();
    }

    private void CreatePoolRoot()
    {
        // PoolRoot라는 빈 게임 오브젝트 생성
        GameObject rootObj = new GameObject("PoolRoot");
        // PoolRoot를 PoolManager 오브젝트의 자식으로 설정
        rootObj.transform.SetParent(transform);
        // 생성한 poolRoot의 트랜스폼 저장
        poolRoot = rootObj.transform;
    }

    public void PreloadPool<T>(T prefab, int count) where T : Component
    {
        // 현재 프리팹의 타입 가져옴
        // Bullet 프리팹 - type(T) = Bullet
        Type type = typeof(T);

        CreatePool(type);

        // count만큼 미리 생성
        for (int i = 0; i < count; i++)
        {
            // 프리팹 이용해 생성
            T obj = Instantiate(prefab);
            // 사용하기 전엔 끄기
            obj.gameObject.SetActive(false);
            // 하이어라잌 정리를 위해 부모 밑으로 이동
            obj.transform.SetParent(poolParents[type]);
            // 생성한 오브젝트를 해당 타입의 큐에 저장
            poolDictionary[type].Enqueue(obj);
        }
    }

    public T GetPool<T>(T prefab) where T : Component
    {
        // 요청한 타입 가져옴
        Type type = typeof(T);

        CreatePool(type);

        // 반환할 오브젝트 변수
        T obj = null;

        // 큐에 대기 중인 오브젝트 있으면 사용
        if (poolDictionary[type].Count > 0)
        {
            obj = poolDictionary[type].Dequeue() as T;
        }
        // 없으면 생성
        else
        {
            obj = Instantiate(prefab);
            // 생성된 오브젝트도 타입별 부모 밑으로 정리
            obj.transform.SetParent(poolParents[type]);
        }

        // 사용할 오브젝트는 활성화
        obj.gameObject.SetActive(true);
        // 꺼낸 오브젝트 반환
        return obj;
    }

    // 사용 끝난 오브젝트를 다시 풀에 넣기
    public void ReturnPool<T>(T obj) where T : Component
    {
        Type type = typeof(T);

        CreatePool(type);

        obj.gameObject.SetActive(false);

        // 다른 부모 밑에 있으면 타입별 풀 부모 밑으로
        if (obj.transform.parent != poolParents[type])
        {
            obj.transform.SetParent(poolParents[type]);
        }

        // 큐에 다시 넣음
        poolDictionary[type].Enqueue(obj);
    }

    private void CreatePool(Type type)
    {
        // 이미 해당 타입의 풀 있으면 return
        if (poolDictionary.ContainsKey(type)) return;

        // 해당 타입의 큐 생성
        poolDictionary.Add(type, new Queue<Component>());
        // 부모 생성 호출
        CreatePoolParent(type);
    }

    private void CreatePoolParent(Type type)
    {
        GameObject parentObj = new GameObject(type.Name);
        parentObj.transform.SetParent(poolRoot);
        poolParents.Add(type, parentObj.transform);
    }
}
