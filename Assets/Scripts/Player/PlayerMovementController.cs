﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    private Rigidbody2D playerBody;
    public CapsuleCollider2D normalCollider;
    public BoxCollider2D crouchCollider;
    public bool isCrouching;
    private PlayerInputController input_Controller;
    public bool ceiling;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float moveSpeed;
    public float cappedVelocity;
    public float jumpPower;
    public float frictionTimeDivider;
    public float cappedJumpVelocity;
    
    public int jumpCounter;
    public int jumpLimit;
    public bool isJumping;

    [SerializeField]
    public bool onGround { get; private set; }

    #region NEED TO FIX

    /*
    Cleaner Code
    Jumping physics(can launch yourself, something to do with physics adding physics. Cyp: When double jump, reset velocity to jump velocity)
    Movement physics
    Can get stuck on side of platforms
    Blue Just gives mask straight away
    Dialogue trees
    Esc to pause
    Esc to exit dialogue
    sound continues to play while in dialogue
    e on dialogue playing loads full dialogue instead of going to next dialogue
    PLAYER WAS THE IMPOSTER
    New Background Parralax

    Crates can be electrified

     */

    #endregion




    void Start()
    {
        
        playerBody = GetComponent<Rigidbody2D>();
        input_Controller = GetComponent<PlayerInputController>();
    }

    public void Move(float horizontalInput)
    {
        
        Vector2 moveVelocity = new Vector2(horizontalInput * (moveSpeed * Time.deltaTime), 0);
        if(Mathf.Abs(playerBody.velocity.x) < cappedVelocity)
        {
            playerBody.velocity += moveVelocity;
        }
    }

    void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < input_Controller.HorizontalThreshold)
        {
            if (Mathf.Abs(playerBody.velocity.x) > .1f)
            {
                float friction = Mathf.Lerp(playerBody.velocity.x, 0, Time.deltaTime / frictionTimeDivider);
                playerBody.velocity = new Vector2(friction, playerBody.velocity.y);
            }
            else
            {
                playerBody.velocity = new Vector2(0, playerBody.velocity.y);
            }
        }


 
    }

    private void FixedUpdate()
    {
        if (Physics2D.Raycast(groundCheck.position, new Vector2(0, -0.2f), 0.1f, groundMask) && playerBody.velocity.y <= 1)
         {
            Debug.Log("IAMGORUDN");
            onGround = true;
            isJumping = false;
            jumpCounter = 0;
        }
        else
        {
            onGround = false;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(groundCheck.position, new Vector3(0, -0.2f));
    }


    public void Jump()
    {
        jumpLimit = input_Controller.switchMask.currentMask == MASKS.DOUBLEJUMP ? 2 : 1;
        if (jumpCounter < jumpLimit)
        {
            isJumping = true;
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            playerBody.velocity += jumpVelocity;
            if (Mathf.Abs(playerBody.velocity.y) > cappedJumpVelocity)
            {
                Debug.Log("Exceeding jump cap velocity is " + Mathf.Abs(playerBody.velocity.y) + " capped velocity " + cappedJumpVelocity);
                playerBody.velocity = new Vector2(playerBody.velocity.x, cappedJumpVelocity);
            }

            jumpCounter++;
        }
    }

    public void Crouch(bool toggle)
    {
        if (toggle)
        {
            crouchCollider.isTrigger = true;
            isCrouching = true;
        }
        else if(!toggle && ceiling)
        {
            crouchCollider.isTrigger = true;
            isCrouching = true;
        } else if(!toggle && !ceiling)
        {
            crouchCollider.isTrigger = false;
            isCrouching = false;
        }
    }

    #region ceilingCheck
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.tag + "STAY");
        if (collision.tag == ("Ceiling"))
        {
            ceiling = true;
        }
        else
        {
            ceiling = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Ceiling"))
        {
            ceiling = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("Ceiling"))
        {
            ceiling = false;
            Crouch(false);
        }
    }
#endregion


    /*
    void OnCollisionEnter2D(Collision2D other)
    {
        if((other.gameObject.tag == ("Ground") || (other.gameObject.GetComponent<Pushable>() && other.gameObject.transform.position.y < this.transform.position.y)))
        {
            onGround = true;
            jumpCounter = 0;
            isJumping = false;
        }

        
    }

    void OnCollisionExit2D(Collision2D other)
    {
        onGround = false;
    }
    */
}
