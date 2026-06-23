using UnityEngine;
using System.Collections;

public class OneBoss : BossBase
{
    [Header("보스 추가 스텟")]
    [SerializeField] public float warningTime = 3f;
    [SerializeField] public float poisonDuration = 5f;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject poisonPrefab;
    protected override void FixedUpdate()
    {
        if (!timerCheck)
        {
            CheckForPlayer();
        }
        if (isKnockedBack || playerTransform == null) return;
        if (isAttack)
        {
            rb.linearVelocity = Vector2.zero;
            Fire();
            return;
        }

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
    }
    private void Fire()
    {
        fireTimer += Time.fixedDeltaTime;
        timerCheck = true;
        if (fireTimer >= enemyFireInterval)
        {

            Vector3 playerPos = playerTransform.position;
            StartCoroutine(PoisonAttackRoutine(playerPos));

            for (int i = 0; i < 2; i++)
            {
                Vector3 randomPos = GetRandomPositionInCamera();
                StartCoroutine(PoisonAttackRoutine(randomPos));
            }

            fireTimer = 0f;
            timerCheck = false;
        }

    }
    private IEnumerator PoisonAttackRoutine(Vector3 targetPos)
    {

        GameObject warning = Instantiate(warningPrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(warningTime);

        Destroy(warning);
        GameObject poison = Instantiate(poisonPrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(poisonDuration);
        Destroy(poison);
    }

    private Vector3 GetRandomPositionInCamera()
    {

        float randomX = Random.Range(0.1f, 0.9f);
        float randomY = Random.Range(0.1f, 0.9f);

        return Camera.main.ViewportToWorldPoint(new Vector3(randomX, randomY, 10f));
    }
}
