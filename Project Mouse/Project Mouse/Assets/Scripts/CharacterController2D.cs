using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterController2D : MonoBehaviour {

    [SerializeField] private float horizontalSpeed = 40.0f;
    [SerializeField] private float aerialSpeed = 40.0f;
    [SerializeField] private float crouchSpeedFactor = 0.33f;
    [SerializeField] private float jumpForce = 300.0f;
    [SerializeField] private float wallJumpForce = 300.0f;
    [SerializeField] private float climbSpeed = 4.0f;

    [Space]
    [Header("Checks:")]
    [SerializeField] private Collider2D groundCheck;
    [SerializeField] private Transform crouchCheck;
    [SerializeField] private Transform climbCheckRoot;
    [SerializeField] private Collider2D topClimbCheck;
    [SerializeField] private Collider2D bottomClimbCheck;
    [SerializeField] private Collider2D crouchDisableCollider;
    [SerializeField] private LayerMask layersToCheckAgainst;

    [Space]
    [Header("Components:")]
    [SerializeField] private SpriteRenderer characterGraphic;
    [SerializeField] private Rigidbody2D rb2d;

    const float COLLISION_CHECK_RADIUS = 0.1f;
    private Vector2 jumpVector = new Vector2();
    private bool isCrouching { get { return !crouchDisableCollider.enabled; } }
    private bool stopCrouchRequested = false;

    public bool isClimbing = false;

    private Vector2 movement = new Vector2();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	private void FixedUpdate () {
        if (isClimbing)
        {
            if(!topClimbCheck.IsTouchingLayers(layersToCheckAgainst) && !bottomClimbCheck.IsTouchingLayers(layersToCheckAgainst))
            {
                StopClimbing();
            }
        }
	}

    public void Move(float hMovement)
    {
        if (isClimbing) { return; }

        if (stopCrouchRequested)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(crouchCheck.position, COLLISION_CHECK_RADIUS, layersToCheckAgainst);
            if(colliders.Length == 0) { StopCrouching(); }
        }

        if(movement.x > 0 && characterGraphic.flipX) {  //If facing left and moving right...
            Flip();
        } else if(movement.x < 0 && !characterGraphic.flipX) { //If facing right and moving left...
            Flip();
        }

        movement = rb2d.velocity;
        if (!IsGrounded())
        {
            movement.x = Mathf.Clamp(movement.x + hMovement * aerialSpeed, -horizontalSpeed * Time.fixedDeltaTime, horizontalSpeed * Time.fixedDeltaTime);
        } else
        {
            movement.x = hMovement * horizontalSpeed;
            
        }

        if (isCrouching) { movement.x *= crouchSpeedFactor; }
        rb2d.velocity = movement;
    }

    private void Flip()
    {
        characterGraphic.flipX = !characterGraphic.flipX;
        climbCheckRoot.localScale = new Vector3(climbCheckRoot.localScale.x * -1f, 1f, 1f);
    }

    public bool IsGrounded()
    {
        return groundCheck.IsTouchingLayers(layersToCheckAgainst);
    }

    public void Jump()
    {
        if (IsGrounded() && !isCrouching && !isClimbing)
        {
            jumpVector.y = jumpForce;
            rb2d.AddForce(jumpVector);
        } else if (isClimbing)
        {
            StopClimbing();
            Flip();
            jumpVector.y = jumpForce;
            jumpVector.x = (characterGraphic.flipX) ? -wallJumpForce : wallJumpForce;
            rb2d.AddForce(jumpVector);
            jumpVector.x = 0f;
        }
    }

    public void Crouch()
    {
        if (!IsGrounded() || isClimbing) { return; }
        stopCrouchRequested = false;
        crouchDisableCollider.enabled = false;
    }

    public void RequestStopCrouch()
    {
        if (!isCrouching) { return; }
        stopCrouchRequested = true;
    }

    private void StopCrouching()
    {
        stopCrouchRequested = false;
        crouchDisableCollider.enabled = true;
    }

    public void Climb(float vMovement)
    {
        if(!topClimbCheck.IsTouchingLayers(layersToCheckAgainst))
        {
            if(bottomClimbCheck.IsTouchingLayers(layersToCheckAgainst) && vMovement > 0)
            {
                vMovement = 0;
            } else 
            {
                return;
            }
        }

        movement.y = vMovement * climbSpeed;
        rb2d.velocity = movement;
    }

    public void StartClimbing()
    {
        if (!topClimbCheck.IsTouchingLayers(layersToCheckAgainst))
        {
            return;
        }

        isClimbing = true;
        movement.x = 0;
        rb2d.gravityScale = 0;
        
    }

    public void StopClimbing()
    {
        isClimbing = false;
        rb2d.gravityScale = 5;
    }
    
}
