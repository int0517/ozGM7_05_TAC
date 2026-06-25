using UnityEngine;
using System.Collections;

public class StateTester : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(TestStateRoutine());
    }

    private IEnumerator TestStateRoutine()
    {
        while (true) // 계속 반복
        {
            // 0: 정지 (5초)
            anim.SetInteger("State", 0);
            Debug.Log("상태: 정지 (0)");
            yield return new WaitForSeconds(5f);

            // 1: 이동 (5초)
            anim.SetInteger("State", 1);
            Debug.Log("상태: 이동 (1)");
            yield return new WaitForSeconds(5f);

            // 2: 맞기 (5초)
            anim.SetInteger("State", 2);
            Debug.Log("상태: 맞기 (2)");
            yield return new WaitForSeconds(5f);

            // 3: 죽기 (5초)
            anim.SetInteger("State", 3);
            Debug.Log("상태: 죽기 (3)");
            yield return new WaitForSeconds(5f);
        }
    }
}
