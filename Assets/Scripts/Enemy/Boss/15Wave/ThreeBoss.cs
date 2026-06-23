using UnityEngine;
using System.Collections;

public class ThreeBoss : BossBase
{
    [Header("보스 추가 스텟")]
    [SerializeField] public float runningSpeed = 15f;
    [SerializeField] private LineRenderer warningLine;
    [SerializeField] private float chargeDuration = 2f;
    [Header("조개 오브젝트")]
    [SerializeField] private GameObject leftShell;  // 왼쪽 조개
    [SerializeField] private GameObject rightShell; // 오른쪽 조개

    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (warningLine != null) warningLine.enabled = false;
    }

    protected override void FixedUpdate()
    {
        if (timerCheck) return;

        if (rb.linearVelocity.magnitude > 0.1f)
        {
            UpdateShells(rb.linearVelocity.x < 0);
        }

        base.FixedUpdate();
    }
    private void UpdateShells(bool isLeft)
    {
        if (leftShell == null || rightShell == null) return;

        leftShell.SetActive(isLeft);
        rightShell.SetActive(!isLeft);
    }
    protected override void CheckForPlayer()
    {
        base.CheckForPlayer();
        if (isAttack && !timerCheck)
        {
            StartCoroutine(ChargeSequence());
        }
    }

    private IEnumerator ChargeSequence()
    {
        timerCheck = true;

        yield return StartCoroutine(ShowWarning());    // 경고 단계
        yield return StartCoroutine(PerformCharge());  // 돌진 단계
        yield return StartCoroutine(ChargeCooldown()); // 대기 단계

        timerCheck = false;
        isAttack = false;
    }

    private IEnumerator ShowWarning()
    {
        warningLine.enabled = true;
        float elapsed = 0f;

        while (elapsed < 2.0f)
        {
            warningLine.SetPosition(0, transform.position);
            warningLine.SetPosition(1, playerTransform.position);
            elapsed += Time.deltaTime;
            yield return null;
        }
        warningLine.enabled = false;
    }

    private IEnumerator PerformCharge()
    {
        Vector2 chargeDir = (playerTransform.position - transform.position).normalized;

        UpdateShells(chargeDir.x < 0);

        rb.linearVelocity = chargeDir * runningSpeed;
        yield return new WaitForSeconds(chargeDuration);
    }

    private IEnumerator ChargeCooldown()
    {
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
    }
}