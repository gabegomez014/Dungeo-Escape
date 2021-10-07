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
    private SpriteRenderer _swordArcSprite;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //Ground layer is currently set to layer 8
        _groundLayer = 1 << 8;

        _anim = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _swordArcSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsGrounded())
        {
            _anim.RegularAttack();
        }

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
            _swordArcSprite.flipX = false;
            _swordArcSprite.flipY = false;

            Vector3 newPos = _swordArcSprite.transform.localPosition;
            newPos.x = 1.01f;

            _swordArcSprite.transform.localPosition = newPos;
        }

        else if (horizontal < 0)
        {
            _playerSprite.flipX = true;
            _swordArcSprite.flipX = true;
            _swordArcSprite.flipY = true;

            Vector3 newPos = _swordArcSprite.transform.localPosition;
            newPos.x = -1.01f;

            _swordArcSprite.transform.localPosition = newPos;
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
