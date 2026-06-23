using JetBrains.Annotations;
using System;
using UnityEngine;

public class pSkill4_Satellites : MonoBehaviour
{
    [Header("스킬 레벨")]
    [SerializeField] private int skill4Level = 0;

    [SerializeField] private GameObject[] satellites;
    [SerializeField] private float rotateSpeed = 100f;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }

    public void Skill4LevelUp()
    {
        if (skill4Level >= 3) return;

        skill4Level++;
        Skill4LevelApply(skill4Level);
    }

    private void Skill4LevelApply(int skillLvl)
    {
        foreach (GameObject satellite in satellites)
        {
            satellite.SetActive(false);
        }

        if (satellites[skillLvl-1] != null) satellites[skillLvl - 1].SetActive(true);

        if (skill4Level == 3)
        {
            rotateSpeed = 150f;
        }
    }
}
