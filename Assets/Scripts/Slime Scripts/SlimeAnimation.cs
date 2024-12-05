using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SlimeAnimation : SlimeCollision
{
    // Reference to slime movement class
    SlimeMovement slime_movement;

    private camera_level_controller level_camera;
    
    void Start()
    {
        StaminaBar_Canvas = GameObject.Find("Bar_Canvas");
        slime_movement = this.gameObject.GetComponent<SlimeMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSticked)
        {
            hasJumped = false;
            if (!onGround)
            {
                if(!SlimeDialogueManager.in_Dialogue)
                    if(!isHardMode)
                        secondsElapsed += Time.deltaTime;
                    else
                        secondsElapsed += Time.deltaTime * 2;

                if (secondsElapsed >= 5)
                {
                    if (fromTop)
                    {
                        TopFall();
                    }
                    else if (fromWalls)
                    {
                        SidewaysFall();
                    }
                    SlimeBody.gravityScale = slime_gravity;
                    slime_movement.toggle_trajectory_arrow(false);

                    isSticked = false;
                }
                else if (secondsElapsed >= 3 && isSticked)
                {
                    if (fromTop)
                    {
                        slimeAnimator.SetBool("isFallingTop", true);
                        slimeAnimator.SetBool("isSideways", false);
                    }
                    if (fromWalls)
                    {
                        slimeAnimator.SetBool("isFallingSideways", true);
                        slimeAnimator.SetBool("isIdle", false);
                        slimeAnimator.SetBool("isSideways", true);
                    }
                }
            }
            Sticky_Regeneration();
        }

        if (!isSticked && !onGround)
        {
            hasJumped = true;
            slimeAnimator.SetBool("isFalling", true);
            slimeAnimator.SetBool("isIdle", false);
            slimeAnimator.SetBool("isSideways", false);
            SlimeBody.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void FixedUpdate()
    {

    }

    private void Sticky_Regeneration()
    {
        if (onGround && secondsElapsed >= 0f)
        {
            secondsElapsed -= Time.deltaTime * 2;
        }
    }

    private void TopFall()
    {
        TopFallingNow();
    }
    private void TopFallingNow()
    {
        SlimeBody.gravityScale = slime_gravity;
        slimeAnimator.SetBool("isFalling", true);
        slimeAnimator.SetBool("isFallingTop", false);
        slimeAnimator.SetBool("isIdle", false);
        slimeAnimator.SetBool("isSideways", false);
        isSticked = false;
    }

    private void SidewaysFall()
    {
        SidewaysFallingNow();
    }
    private void SidewaysFallingNow()
    {
        SlimeBody.gravityScale = slime_gravity;
        slimeAnimator.SetBool("isFalling", true);
        slimeAnimator.SetBool("isFallingSideways", false);
        slimeAnimator.SetBool("isIdle", false);
        slimeAnimator.SetBool("isSideways", false);
        isSticked = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
