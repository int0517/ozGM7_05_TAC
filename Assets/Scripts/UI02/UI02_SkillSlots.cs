using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UI02_SkillSlots : MonoBehaviour
{
    [SerializeField] private Image[] skillSlots;
    [SerializeField] private Sprite testSkillIcon;

    private void Start()
    {
        skillSlots[0].sprite = testSkillIcon;
    }

}
