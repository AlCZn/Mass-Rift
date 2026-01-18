using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("ÒÆ¶¯²ÎÊý")]
    [SerializeField] private float moveSpeed = 5f;

    private float horizontalInput;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        Move();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    void Move()
    {
        float horizontalVelocity = horizontalInput * moveSpeed;
        float verticalVelocity = rb.velocity.y;
        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }
}
