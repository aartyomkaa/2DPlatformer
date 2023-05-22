using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float MaxSpeed = 5f;
    public float JumpSpeed = 5f;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && Grounded) 
        {
            Velocity.y = JumpSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Velocity.y > 0)
            {
                float jumpReduce = 0.5f;
                Velocity.y = Velocity.y * jumpReduce;
            }
        }

        bool flipSprite = (_spriteRenderer.flipX ? (move.x > 0.0f) : (move.x < 0.0f));

        if (flipSprite)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        _animator.SetFloat("Speed", Mathf.Abs(Velocity.x) / MaxSpeed);

        TargetVelocity = move * MaxSpeed;
    }
}
