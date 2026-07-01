using UnityEngine;
using static PlayerAnimationController;

public class HitEndEvent : MonoBehaviour
{
    [SerializeField] private PlayerStat pStat;

    public void HitEnd()
    {
        pStat.EndHit();
    }
}
