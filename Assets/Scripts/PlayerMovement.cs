using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator myAnimator;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawnPoint;
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    bool isJumping = false;
    bool isAlive = true;

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    void Update()
    {
        if (isAlive) {
            Run();
            FlipSprite();
            ClimbLadder();
            Die();
        }
    }

    void OnMove(InputValue value)
    {
        if (isAlive) {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    {
        if (isAlive) {
            if (value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing"))) {
                    myRigidBody.velocity += new Vector2(0, jumpSpeed);
                    isJumping = true;
            }
        }
    }

    void OnFire(InputValue value)
    {
        if (isAlive) {
            GameObject g = Instantiate<GameObject>(projectile, projectileSpawnPoint.position, transform.rotation);
        }
    }

    void Run()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) isJumping = false;

        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        myAnimator.SetBool("isRunning", MathF.Abs(myRigidBody.velocity.x) > Mathf.Epsilon);
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = MathF.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")) &&!isJumping) {
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
            myRigidBody.velocity = climbVelocity;
            myRigidBody.gravityScale = 0f;
            myAnimator.SetBool("isClimbing", MathF.Abs(myRigidBody.velocity.y) > Mathf.Epsilon);
        } else {
            isJumping = false;
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
        }
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
