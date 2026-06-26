using UnityEngine;
using SP1Assets.MonsterPack2D;

public class test1 : MonoBehaviour
{
    MonsterPrefabController monster;

    void Start()
    {
        monster = GetComponent<MonsterPrefabController>();
        Debug.Log(monster);
        monster.Init();

        foreach (string anim in monster.GetAnimationNames())
        {
            Debug.Log(anim);
        }
        monster.SetAnimationSpeed(0.5f);
        monster.PlayAnimation("walk", 3.0f);
    }

    void Update()
    {
        //transform.Translate(Vector2.left * 2f * Time.deltaTime);

        
    }

}
