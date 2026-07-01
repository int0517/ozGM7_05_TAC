using UnityEngine;

public class BulletTest : MonoBehaviour
{
    float maxtimer = 3f;
    float timer = 3f;
    Vector3 moveDir = new Vector3(0.01f, 0f, 0f);

    void Update()
    {
        transform.position += moveDir;

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            moveDir.x *= -1f;
            timer = maxtimer;
        }
    }
}
