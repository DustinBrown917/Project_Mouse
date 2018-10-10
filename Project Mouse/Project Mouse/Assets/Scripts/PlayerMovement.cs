using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] CharacterController2D controller;

    private bool hadHMovementLastFrame = false;
    private float hMovement = 0.0f;
    private float vMovement = 0.0f;
    private bool callForJump = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
		hMovement = Input.GetAxis("Horizontal");
        vMovement = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump")) { callForJump = true; }

        if (Input.GetButtonDown("Crouch")) { controller.Crouch(); }
        else if (Input.GetButtonUp("Crouch")) { controller.RequestStopCrouch(); }
	}

    private void FixedUpdate()
    {
        if(Mathf.Abs(hMovement) > 0.0f)
        {
            controller.Move(hMovement * Time.fixedDeltaTime);
            hadHMovementLastFrame = true;
        } else if (hadHMovementLastFrame)
        {
            controller.Move(0.0f);
            hadHMovementLastFrame = false;
        }

        if (callForJump) {
            controller.Jump();
            callForJump = false;    
        }

        if (controller.isClimbing)
        {
            controller.Climb(vMovement * Time.fixedDeltaTime);
        } else if (vMovement > 0.0f)
        {
            controller.StartClimbing();
        }
        
    }
}
