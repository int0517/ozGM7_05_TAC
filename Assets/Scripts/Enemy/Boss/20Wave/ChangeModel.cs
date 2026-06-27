using UnityEngine;
using UnityEngine.U2D.Animation;

public class ChangeModel : MonoBehaviour
{
    [Header("º¸½º ÆÄÃ÷")]
    [SerializeField] private SpriteResolver eyeResolver;
    [SerializeField] private SpriteResolver leftArmResolver;
    [SerializeField] private SpriteResolver rightArmResolver;
    [SerializeField] private SpriteResolver clothResolver;
    [SerializeField] private SpriteResolver bodyResolver;
    [SerializeField] private SpriteResolver headResolver;
    [SerializeField] private SpriteResolver leftLegResolver;
    [SerializeField] private SpriteResolver rightLegResolver;
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
