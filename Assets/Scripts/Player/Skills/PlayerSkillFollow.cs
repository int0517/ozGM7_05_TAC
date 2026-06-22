using UnityEngine;

public class PlayerSkillFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    void Update()
    {
        transform.position = playerTransform.position;
    }
}
