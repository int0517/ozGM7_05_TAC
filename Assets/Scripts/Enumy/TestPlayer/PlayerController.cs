using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Transform firePoint;
    public GameObject bulletPrefab;
    private float fireInterval = 1.5f;
    private float FireTimer = 0.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        moveInput = InputManger.Movement;

        FireTimer += Time.deltaTime;
        if (FireTimer >= fireInterval)
        {
            Fire();
            FireTimer = 0.0f;
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
    private void Fire()
    {

        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }

}
