using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 25f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    // private static GameSession instance;

    Vector2 moveInput;
    Rigidbody2D myrigidbody2D;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider2D;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    bool isTouchingWall = false; // New variable to track if the player is touching a wall
    bool isAlive = true;

    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    // Start is called before the first frame update

    void Start()
    {
        myrigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myrigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; };
        Die();
        Run();
        FlipSprite();
        ClimbLadder();
        CheckIfGrounded(); // New method to check if the player is on the ground
    }


    private void Run()
    {
        if (!isAlive) { return; };
        if (isTouchingWall) // Prevent running if touching a wall
        {
            myrigidbody2D.velocity = new Vector2(0, myrigidbody2D.velocity.y);
            myAnimator.SetBool("isRunning", false);
            return;
        }

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myrigidbody2D.velocity.y);
        myrigidbody2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myrigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myrigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myrigidbody2D.velocity.x), 1f);
        }
    }

    void OnFire(InputValue value)
    {

        if (!isAlive) { return; };
        Instantiate(bullet, gun.position, transform.rotation);

    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; };
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; };
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myrigidbody2D.velocity += new Vector2(0f, jumpSpeed);
        }

        // Check if the player touches a wall while jumping
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Wall")))
        {
            isTouchingWall = true;
        }
    }

    private void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myrigidbody2D.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);  // Add animation for climbing
            return;
        }

        Vector2 climbVelocity = new Vector2(myrigidbody2D.velocity.x, moveInput.y * climbSpeed);
        myrigidbody2D.velocity = climbVelocity;
        myrigidbody2D.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myrigidbody2D.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    private void CheckIfGrounded()
    {
        // Check if the player is touching the ground
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isTouchingWall = false;
        }
    }
    private void Die()
    {

        if (myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myrigidbody2D.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();

        }
    }
}
