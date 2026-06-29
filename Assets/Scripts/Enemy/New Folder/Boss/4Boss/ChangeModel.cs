using UnityEngine;
using UnityEngine.U2D.Animation;

public class ChangeModel : MonoBehaviour
{
    [Header("º¸½º ÆÄÃ÷")]
    [Header("´«")]
    [SerializeField] private SpriteResolver eyeResolver;
    [Header("¾ó±¼")]
    [SerializeField] private SpriteResolver headResolver;
    [Header("¸öÅë")]
    [SerializeField] private SpriteResolver bodyResolver;
    [Header("¿Ê")]
    [SerializeField] private SpriteResolver clothResolver;
    [Header("¿Þ¼Õ")]
    [SerializeField] private SpriteResolver leftArmResolver;
    [Header("¿À¸¥¼Õ")]
    [SerializeField] private SpriteResolver rightArmResolver;
    [Header("¿Þ´Ù¸®")]
    [SerializeField] private SpriteResolver leftLegResolver;
    [Header("¿À¸¥´Ù¸®")]
    [SerializeField] private SpriteResolver rightLegResolver;
    [Header("¸ùµÕÀÌ")]
    [SerializeField] private GameObject club;

    private bool isChanged = false;

    public void ChangeForm()
    {
        if (isChanged) return;
        isChanged = true;

        eyeResolver.SetCategoryAndLabel("eyes", "eyes_0005");
        leftArmResolver.SetCategoryAndLabel("arm_L", "arm_L_0005");
        rightArmResolver.SetCategoryAndLabel("arm_R", "arm_R_0005");
        clothResolver.SetCategoryAndLabel("cloth", "cloth_0005");
        bodyResolver.SetCategoryAndLabel("body", "body_0005");
        headResolver.SetCategoryAndLabel("head", "head_0005");
        leftLegResolver.SetCategoryAndLabel("leg_L", "leg_L_0005");
        rightLegResolver.SetCategoryAndLabel("leg_R", "leg_R_0005");
        club.SetActive(true);
    }
}
