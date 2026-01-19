using System;
using System.Net;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    [SerializeField] private float moveSpeed = 8f;        // 移动速度
    [SerializeField] private float jumpForce = 16f;       // 跳跃力度

    [Header("地面检测")]
    [SerializeField] private Transform groundCheck;       // 地面检测点
    [SerializeField] private float groundCheckRadius = 0.2f;  // 检测半径
    [SerializeField] private LayerMask groundLayer;       // 地面层级

    // 组件引用
    private Rigidbody2D rb;

    // 输入
    private float horizontalInput;  // 移动输入
    private bool jumpInput;  // 跳跃输入
    private bool isMouseDown;  // 鼠标是否按下
    private bool isMouseUp;  // 鼠标是否离开
    private Vector3 mouseCoordinates;  // 鼠标坐标

    // 状态
    private bool isGrounded;
    private bool canJump = true;  // 一段跳限制

    // 发射线
    private float minLength;  // 最小长度
    private float maxLength;  // 最大长度
    private Vector2 currentLine;

    private void Awake()
    {
        // 获取组件
        rb = GetComponent<Rigidbody2D>();

        // 如果未指定地面检测点，使用自身位置
        if (groundCheck == null)
        {
            groundCheck = transform;
        }
    }

    private void Update()
    {
        // 获取输入
        GetInput();

        // 检测地面
        CheckGrounded();

        // 处理跳跃输入
        if (jumpInput && isGrounded && canJump)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // 应用移动
        Move();
    }

    /// <summary>
    /// 获取玩家输入
    /// </summary>
    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        isMouseDown = Input.GetMouseButton(0);
        mouseCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isMouseUp = Input.GetMouseButtonUp(0);
    }

    /// <summary>
    /// 检测是否在地面
    /// </summary>
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 当接触地面时重置跳跃能力
        if (isGrounded && rb.velocity.y <= 0)
        {
            canJump = true;
        }
    }

    /// <summary>
    /// 移动角色
    /// </summary>
    private void Move()
    {
        // 计算水平速度
        float horizontalVelocity = horizontalInput * moveSpeed;

        // 保持当前垂直速度
        float verticalVelocity = rb.velocity.y;

        // 设置新速度
        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    /// <summary>
    /// 执行跳跃
    /// </summary>
    private void Jump()
    {
        // 应用跳跃力
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // 禁用再次跳跃（一段跳限制）
        canJump = false;
    }

    /// <summary>
    /// 在编辑器中绘制地面检测范围
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}