using UnityEngine;

public class UI02_SkillDataManager : MonoBehaviour
{
    [CreateAssetMenu(fileName = "New SkillData", menuName = "Game/SkillData")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
}
}
