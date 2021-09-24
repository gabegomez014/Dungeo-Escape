using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private float _jumpForce = 10;

    //Get reference to RigidBody
    private Rigidbody2D _rb;
    private Vector2 _currentVelocity;
    private int _groundLayer;
    private bool _resetJump;

    private PlayerAnimation _anim;
    private SpriteRenderer _playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //Ground layer is currently set to layer 8
        _groundLayer = 1 << 8;

        _anim = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        // Horizontal input for left/right
        float horizontal = Input.GetAxisRaw("Horizontal");
        bool isGrounded = IsGrounded();

        if (horizontal > 0)
        {
            _playerSprite.flipX = false;
        }

        else if (horizontal < 0)
        {
            _playerSprite.flipX = true;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            _anim.Jumping();
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            StartCoroutine(ResetJumpRoutine());
        }

        _rb.velocity = new Vector2(horizontal * _speed, _rb.velocity.y);
        _anim.Move(horizontal);
    }

    bool IsGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, _groundLayer);

        if (hitInfo.collider != null)
        {
            if (!_resetJump)
            {
                _anim.NotJumping();
                return true;
            }
        }

        return false;
    }

    IEnumerator ResetJumpRoutine()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }
}
