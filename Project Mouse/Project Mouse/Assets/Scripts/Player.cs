using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float horizontalSpeed = 1;
    [SerializeField]
    private float jumpForce = 10;
    private Rigidbody2D rb2d;
    private Vector2 newVel;
    private Direction movementDirection = Direction.NONE;
    private Direction facingDirection = Direction.RIGHT;

    private Animator animator;

    [SerializeField]
    private SpriteRenderer characterSprite;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        HandleHorizontalMovement();
        HandleVerticalMovement();
        rb2d.velocity = newVel;
        FindMovementDirection();

    }

    private void HandleHorizontalMovement()
    {
        newVel.x = Input.GetAxis("Horizontal") * horizontalSpeed;

        if(newVel.x == 0) {
            return;
        }
    }

    private void HandleVerticalMovement()
    {
        newVel.y = rb2d.velocity.y;

        if (Input.GetButtonDown("Jump"))
        {
            newVel.y = jumpForce;
        }
    }

    /// <summary>
    /// Sets the movementDirection variable.
    /// </summary>
    /// <param name="direction">The direction to set movementDirection to.</param>
    private void SetMovementDirection(Direction direction)
    {
        if(movementDirection == direction) {
            return;
        } else {
            movementDirection = direction;
            SetAnimation();
        }
    }


    /// <summary>
    /// Finds what direction the RigidBody2D is moving in.
    /// </summary>
    private void FindMovementDirection()
    {
        if (rb2d.velocity.x == 0 && rb2d.velocity.y == 0) {
            SetMovementDirection(Direction.NONE);
        }
        else if (rb2d.velocity.x > 0) {
            if(rb2d.velocity.y > 0) {
                SetMovementDirection(Direction.UP_RIGHT);
            } else if(rb2d.velocity.y < 0) {
                SetMovementDirection(Direction.DOWN_RIGHT);
            } else {
                SetMovementDirection(Direction.RIGHT);
            }
            facingDirection = Direction.RIGHT;
        }
        else if(rb2d.velocity.x < 0) {
            if (rb2d.velocity.y > 0) {
                SetMovementDirection(Direction.UP_LEFT);
            } else if (rb2d.velocity.y < 0) {
                SetMovementDirection(Direction.DOWN_LEFT);
            } else {
                SetMovementDirection(Direction.LEFT);
            }
            facingDirection = Direction.LEFT;
        }
        else if(rb2d.velocity.x == 0) {
            if(rb2d.velocity.y > 0) {
                SetMovementDirection(Direction.UP);
            } else {
                SetMovementDirection(Direction.DOWN);
            }
        }
    }

    private void SetAnimation()
    {
        switch (movementDirection)
        {
            case Direction.NONE:
                if(facingDirection == Direction.RIGHT) { animator.Play("idle_right_0"); }
                else if(facingDirection == Direction.LEFT) { animator.Play("idle_left_0"); }
                break;
            case Direction.RIGHT:
                animator.Play("run_right_0");
                break;
            case Direction.LEFT:
                animator.Play("run_left_0");
                break;
            case Direction.UP:
                //Set upward animation.
                break;
            case Direction.DOWN:
                //Set downward animation.
                break;
            case Direction.UP_RIGHT:
                //Set upright animation.
                break;
            case Direction.UP_LEFT:
                //Set upleft animation.
                break;
            case Direction.DOWN_RIGHT:
                //Set downright animation.
                break;
            case Direction.DOWN_LEFT:
                //Set downleft animation.
                break;
            default:
                //Set idle animation.
                break;
        }
    }

}
