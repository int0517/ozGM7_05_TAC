using UnityEngine;

public class PlayerSkillLevel : MonoBehaviour
{
    [SerializeField] private PlayerNormalAttack normalAttack;
    [SerializeField] private pSkill1_FireBall pSkill1;
    [SerializeField] private pSkill2_FreezeArea pSkill2;
    [SerializeField] private pSkill3_Laser pSkill3;
    [SerializeField] private pSkill4_Satellites pSkill4;
    [SerializeField] private pSkill5_AcidRain pSkill5;

    public int GetSkillLvl(PlayerStatEnum skillEnum)
    {
        switch(skillEnum)
        {
            case PlayerStatEnum.NormalAttack:
                return normalAttack.NormalAttackLevel;
            case PlayerStatEnum.Skill1:
                return pSkill1.Skill1Level;
            case PlayerStatEnum.Skill2:
                return pSkill2.Skill2Level;
            case PlayerStatEnum.Skill3:
                return pSkill3.Skill3Level;
            case PlayerStatEnum.Skill4:
                return pSkill4.Skill4Level;
            case PlayerStatEnum.Skill5:
                return pSkill5.Skill5Level;
            default:
                return -1;
        }
    }
}
