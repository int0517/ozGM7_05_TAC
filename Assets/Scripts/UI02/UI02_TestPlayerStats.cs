using TMPro;
using UnityEngine;

public class UI02_TestPlayerStats : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text attackSpeed;
    [SerializeField] private TMP_Text maxHpText;
    [SerializeField] private TMP_Text moveSpeed;
    [SerializeField] private TMP_Text magemtism;

    [Header("Test Data")]
    [SerializeField] private int testScore = 12345;
    [SerializeField] private int testAttack = 25;
    [SerializeField] private int testAttackSpeed = 7;
    [SerializeField] private int testMaxHp = 100;
    [SerializeField] private int testMoveSpeed = 7;
    [SerializeField] private int testMagemtism = 30;

    private void Start()
    {
        scoreText.text = $"SCORE : {testScore}";

        attackText.text = $"ATK : {testAttack}";
        attackSpeed.text = $"ATK SPD : {testAttackSpeed}";
        maxHpText.text = $"MAX HP : {testMaxHp}";
        moveSpeed.text = $"MOVE SPD : {testMoveSpeed}";
        magemtism.text = $"MAGNETISM : {testMagemtism}";
    }
}