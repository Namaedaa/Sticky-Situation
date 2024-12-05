using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeCollision : SlimeMovement
{
    // Slime Variables
    protected CircleCollider2D SlimeCollider;

    // Camera Variables

    protected GameObject RespawnBox;

    protected GameObject DeathBox;

    protected Animator CameraStates;

    protected CinemachineVirtualCamera DeathCamera;

    // Audio Variables

    public AudioSource BumpSfx;

    public AudioSource DeathSfx;


    // Stamina Bar

    public GameObject StaminaBar_Canvas;

    private float collision_time = 0f;

    //Health
    HealthScript healthScript;
    public GameObject deathFX;

    //Attack Enemies
    [SerializeField]
    private float damageAmount;

    public static bool end_collider_fix = false;


    void Start()
    {
        RespawnBox = GameObject.Find("Respawn Area");
        healthScript = this.gameObject.GetComponent<HealthScript>();
        DeathBox = GameObject.Find("Death Area");

        CameraStates = GameObject.Find("Camera States").GetComponent<Animator>();
        DeathCamera = GameObject.Find("Death Camera").GetComponent<CinemachineVirtualCamera>();

        StaminaBar_Canvas = GameObject.Find("Bar_Canvas");

        SlimeCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isSticked && !isHolding)
        {
            if (last_collision_direction == "Right")
            {
                simple_XOrientation("Right");

                slimeAnimator.SetBool("isSideways", true);
                slimeAnimator.SetBool("isFalling", false);
                slimeAnimator.SetBool("isIdle", false);
            }
            if (last_collision_direction == "Left")
            {
                simple_XOrientation("Left");

                slimeAnimator.SetBool("isSideways", true);
                slimeAnimator.SetBool("isFalling", false);
                slimeAnimator.SetBool("isIdle", false);
            }
            if (last_collision_direction == "Top")
            {
                simple_YOrientation("Top");

                slimeAnimator.SetBool("isIdle", true);
                slimeAnimator.SetBool("isSideways", false);
                slimeAnimator.SetBool("isFalling", false);
            }
            if (last_collision_direction == "Bottom")
            {
                simple_YOrientation("Bottom");

                slimeAnimator.SetBool("isIdle", true);
                slimeAnimator.SetBool("isSideways", false);
                slimeAnimator.SetBool("isFalling", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasDied)
        {
            end_collider_fix = false;
            Vector2 contactPoint = collision.GetContact(0).normal;

            hasJumped = false;
            hasdoubleJumped = false;
            collision_time = 0f;

            if (collision.collider.tag == "EnemyProjectile" || collision.collider.tag == "EnemyAttack")
            {
                Knockback_FromEnemy(collision);
                collision.collider.GetComponent<EnemyAttack>().Attack();
                /* Kill_Player();*/
            }

            else if (collision.collider.tag == "Spikes")
            {
                collision.collider.GetComponent<EnemyAttack>().Attack();
                SlimeBody.gravityScale = slime_gravity;
                jump_falling = true;

                ContactCollision(contactPoint);
            }
            else if(collision.collider.tag == "Enemy" || collision.collider.tag == "Boss")
            {
                Knockback_FromEnemy(collision);
                SlimeBody.gravityScale = slime_gravity;
                var healthController = collision.gameObject.GetComponent<HealthScript>();
                healthController.TakeDamage(damageAmount);

            }
            else if(collision.collider.tag == "Wall")
            {
                jump_falling = false;
                BumpSfx.Play();

                ContactCollision(contactPoint);
            }
        }
    }

    private void ContactCollision(Vector2 contactPoint)
    {
        if (contactPoint.x >= -1 && contactPoint.x <= -0.7)
        {
            simple_XOrientation("Right");
            SlimeBody.gravityScale = 0;
            SlimeBody.velocity = new Vector2(0, 0);

            slimeAnimator.SetBool("isSideways", true);
            slimeAnimator.SetBool("isFalling", false);
            slimeAnimator.SetBool("isIdle", false);
            fromTop = false;
            fromWalls = true;
            isSticked = true;
            onGround = false;
        }
        else if (contactPoint.x >= 0.7 && contactPoint.x <= 1)
        {
            simple_XOrientation("Left");
            SlimeBody.gravityScale = 0;
            SlimeBody.velocity = new Vector2(0, 0);
            slimeAnimator.SetBool("isSideways", true);
            slimeAnimator.SetBool("isFalling", false);
            slimeAnimator.SetBool("isIdle", false);
            fromTop = false;
            fromWalls = true;
            isSticked = true;
            onGround = false;

        }
        else if (contactPoint.y >= -1 && contactPoint.y <= -0.7)
        {
            simple_YOrientation("Top");
            SlimeBody.gravityScale = 0;
            SlimeBody.velocity = new Vector2(0, 0);
            slimeAnimator.SetBool("isIdle", true);
            slimeAnimator.SetBool("isSideways", false);
            slimeAnimator.SetBool("isFalling", false);

            fromTop = true;
            fromWalls = false;
            isSticked = true;
            onGround = false;

        }
        else if (contactPoint.y >= 0.7 && contactPoint.y <= 1)
        {
            simple_YOrientation("Bottom");
            SlimeBody.gravityScale = 0;
            SlimeBody.velocity = new Vector2(0, 0);
            slimeAnimator.SetBool("isIdle", true);
            slimeAnimator.SetBool("isSideways", false);
            slimeAnimator.SetBool("isFalling", false);

            fromTop = false;
            fromWalls = false;
            isSticked = true;
            onGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Checkpoint")
        {
            if(collision.gameObject.transform.childCount >= 1)
                try
                {
                    RespawnBox.transform.position = collision.gameObject.transform.GetChild(0).transform.position;
                }
                catch
                {

                }
        }

        if (!trajectory_colliding)
        {
            if (collision.tag == "Trajectory End")
            {
                SlimeBody.gravityScale = slime_gravity;
                jump_falling = true;
            }
        }
        

        if (collision.tag == "EnemyProjectile" || collision.tag == "Spikes" || collision.tag == "EnemyAttack")
        {
            collision.GetComponent<EnemyAttack>().Attack();
            /*Kill_Player();*/
        }

        if(collision.tag == "Camera Level")
        {
            CinemachineVirtualCamera camera_entered = collision.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
            camera_entered.Priority = 13;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Camera Level")
        {
            CinemachineVirtualCamera camera_entered = collision.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
            camera_entered.Priority = 11;
        }

        if (collision.tag == "Trajectory End" && end_collider_fix && !isSticked)
        {
            SlimeBody.gravityScale = slime_gravity;
            jump_falling = true;
            end_collider_fix = false;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Trajectory End" && !isSticked)
        {
            end_collider_fix = true;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        StartCoroutine(check_stay_collision(collision));
    }

    IEnumerator check_stay_collision(Collision2D collision)
    {
        if (hasJumped && collision.collider.tag == "Wall")
        {
            collision_time += Time.deltaTime;
            if (collision_time >= .3f)
            {
                try
                {
                    OnCollisionEnter2D(collision);
                    hasJumped = false;
                    collision_time = 0f;
                }
                catch
                {
                }
            }
        }
        else
            collision_time = 0f;
        yield return null;
        
    }



    public void Kill_Player()
    {
        DeathBox.transform.position = this.transform.position;

        StaminaBar_Canvas.SetActive(false);

        CameraStates.SetBool("isDead", true);
        toggle_trajectory_arrow(false);
        /*SlimeCollider.enabled = false;*/
        DeathSfx.Play();

        slimeAnimator.SetBool("isDead", true);
        SlimeBody.velocity = new Vector2(0, 0);
        slimeAnimator.SetBool("isSideways", false);
        slimeAnimator.SetBool("isFalling", false);

        SlimeBody.gravityScale = slime_gravity;

        hasDied = true;
        hasJumped = false;

        Invoke("Respawn", 2f);
    }

    private void Knockback_FromEnemy(Collision2D collision)
    {
        Vector2 KnockbackDir = (SlimeBody.transform.position - collision.transform.position).normalized * 15;
        SlimeBody.velocity = KnockbackDir;
        SlimeBody.gravityScale = slime_gravity;
        jump_falling = true;
    }

    private void Respawn()
    {
        SlimeBody.velocity = new Vector2(0, 0);
        //adds Health
        
        healthScript.AddHealth(100);
        deathFX.SetActive(false);
        slimeAnimator.SetBool("isIdle", false);
        slimeAnimator.SetBool("isDead", false);
        slimeAnimator.SetBool("isSideways", false);
        slimeAnimator.SetBool("isFalling", true);

        StaminaBar_Canvas.SetActive(true);

        hasDied = false;

        fromTop = false;
        fromWalls = false;
        isSticked = false;
        onGround = false;

        SlimeBody.transform.position = RespawnBox.transform.position;
        CameraStates.SetBool("isDead", false);
        SlimeCollider.enabled = true;


        DeathCamera.Follow = DeathBox.transform;
    }

    private void OnBecameInvisible()
    {
        
    }

}
