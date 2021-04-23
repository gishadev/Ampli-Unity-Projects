using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;

    [Header("GroundCheck")]
    public Transform checkPoint;
    public float checkRadius;
    public LayerMask whatIsGround;

    float _hInput;
    float _rawInput;
    bool _isGrounded = true;

    Rigidbody2D _rb;
    Animator _animator;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _hInput = Input.GetAxis("Horizontal");
        _rawInput = Input.GetAxisRaw("Horizontal");

        CheckForGround();

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            Jump();

        FlipSprite();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rb.velocity = new Vector2(_hInput * moveSpeed, _rb.velocity.y);
    }

    private void Jump()
    {
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckForGround()
    {
        Collider2D coll = Physics2D.OverlapCircle(checkPoint.position, checkRadius, whatIsGround);

        if (coll != null)
            _isGrounded = true;
        else
            _isGrounded = false;
    }

    private void FlipSprite()
    {
        if (_rawInput > 0f)
            _spriteRenderer.flipX = false;
        else if (_rawInput < 0f)
            _spriteRenderer.flipX = true;
    }

    private void HandleAnimation()
    {
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetBool("IsRunning", Mathf.Abs(_rawInput) > 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checkPoint.position, checkRadius);
    }
}
