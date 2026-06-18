using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class UI02_SkillIcons : MonoBehaviour
{
    [SerializeField] private List<GameObject> ownedSkills;

    [System.Serializable]
    public class SkillData
    {
        public string skillName;
        public string description;
        public Sprite icon;
    }
}
