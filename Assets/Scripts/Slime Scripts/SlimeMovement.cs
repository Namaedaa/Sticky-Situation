using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
/*using UnityEditor.ShaderGraph.Internal;*/
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using System.Linq;

public class SlimeMovement : MonoBehaviour
{
    //Misc Variables

    public Transform Slime_Transform;

    // Slime Variables

    protected static Rigidbody2D SlimeBody;

    protected static SpriteRenderer slimeSprite;

    protected static Animator slimeAnimator;

    //Collision Variables

    public static bool hasDied = false;

    protected static bool isSticked = false;

    protected static bool onGround = false;

    protected static bool hasJumped = false;

    protected static bool fromTop = false;

    protected static bool fromWalls = false;

    public static float secondsElapsed = 0f;


    // Launch Variables

    float length_of_line;

    protected static bool isDragging = false;

    protected static bool hasdoubleJumped = false;

    private Vector2 Starting_Position;

    [SerializeField]
    protected LineRenderer Trajectory_Line;

    public float trajectory_launch_power = 0f;

    [SerializeField]
    private float clamp_distance = 5f;

    [SerializeField]
    internal float slime_gravity = 6f;

    private Vector2 final_direction;

    private Vector2 line_end;

    private LayerMask wall_layer;

    private Vector2 collided_final_pos;

    public static bool trajectory_colliding = false;

    public Transform trajectory_arrow;

    public ParticleSystem jumpEffect;
 

    Vector2 velocity;

    Vector2 Current_mouse_pos;

    internal static bool jump_falling = false;

    public float horizontal_drag = 0.01f;

    public Transform end_collider;



    // Sound Variables

    public AudioSource JumpSfx;

    // Hard Mode variables

    public static bool isHardMode = false;

    //Controls for Controllers
    Vector2 lStick, mousePosition;

    Gameplay controls;

    internal static bool isHolding = false;

    private bool isReleased = false;

    private string current_controller = "";

    private PlayerInput player_input;

    public PauseManager pauseManager;

    public static string last_collision_direction = "";

    void Start()
    {
        player_input = this.GetComponent<PlayerInput>();
        player_input.enabled = true;
        //Controller related shizz
        controls = new Gameplay();
        controls.Enable();
        controls.PlayerControls.Enable();
       /*controls.PlayerControls.Move.performed += move_performing;*/
        controls.PlayerControls.Jump.performed += jump_holding;
        controls.PlayerControls.Jump.canceled += jump_canceled;
        controls.PlayerControls.OpenCloseMenu.performed += pause_unpause;

        setup_hard_mode();


        SlimeBody = GetComponent<Rigidbody2D>();
        slimeAnimator = GetComponent<Animator>();
        slimeSprite = GetComponent<SpriteRenderer>();

        wall_layer = LayerMask.GetMask("Wall");
    }

    private void setup_hard_mode()
    {
        HealthScript slime_health_script = GetComponent<HealthScript>();
        if (isHardMode)
        {
            slime_health_script.maximumHealth = 10f;
            slime_health_script.currentHealth = 10f;
        }
        else
        {
            slime_health_script.maximumHealth = 100f;
            slime_health_script.currentHealth = 100f;
        }
    }

    private void jump_holding(InputAction.CallbackContext context)
    {
        if (!PauseManager.game_paused)
        {
            if (!SlimeDialogueManager.in_Dialogue)
            {
                if (!hasDied)
                {
                    if (!IsPointerOverUI())
                    {
                        if (!hasJumped || !hasdoubleJumped)
                        {
                            toggle_trajectory_arrow(true);
                        
                            isHolding = true;
                        }

                        current_controller = context.control.name;
                    }
                }
            }
        }
    }

    private void pause_unpause(InputAction.CallbackContext context)
    {
        if (!PauseManager.game_paused)
        {
            pauseManager.pause_game();
        }
        else
        {
            pauseManager.resume_game();
        }
    }
    private void jump_canceled(InputAction.CallbackContext context)
    {
        if (!PauseManager.game_paused)
        {
            if (!SlimeDialogueManager.in_Dialogue)
            {
                if ((!hasJumped || !hasdoubleJumped) && isHolding) {
                    isReleased = true;
                    PlayJumpEffect();
                }
            }
        }
    }

    public static bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults.Any(x => LayerMask.LayerToName(x.gameObject.layer) == "UI");

    }

    public static Vector2 ClampMagnitude(Vector2 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }
    void Update()
    {
        StartCoroutine("check_ifStuck");

        if (PauseManager.game_paused)
        {
            return;
        }

        if (SlimeDialogueManager.in_Dialogue)
        {
            toggle_trajectory_arrow(false);
            return;
        }

        if (hasDied)
        {
            return;
        }

        if (!hasJumped || !hasdoubleJumped)
        {
            // Trajectory Line Code 

            if (isHolding)
            {
                jump_falling = false;

                Starting_Position = Slime_Transform.position;

                trajectory_colliding = false;
                Vector2 trajectory_direction;
                if (current_controller == "leftButton")
                {
                    lStick = Mouse.current.position.ReadValue();
                    Current_mouse_pos = Camera.main.ScreenToWorldPoint(lStick);
                    trajectory_direction = (Current_mouse_pos - Starting_Position);
                    line_end = Starting_Position + ClampMagnitude(trajectory_direction, clamp_distance, 1f);
                }
                else
                {
                    lStick = controls.PlayerControls.Move.ReadValue<Vector2>();
                    line_end = Starting_Position + ClampMagnitude(lStick * 5, clamp_distance, 1f);
                }

                //testing some stuff
                final_direction = line_end - Starting_Position;

                length_of_line = Vector3.Distance(Slime_Transform.position, line_end);

                //orig
                RaycastHit2D raycast_hit = Physics2D.Raycast(Slime_Transform.position, final_direction, length_of_line, wall_layer);
                if (raycast_hit.collider != null)
                {
                    line_end = raycast_hit.point;
                    trajectory_colliding = true;
                }


                velocity = (line_end - Starting_Position) * trajectory_launch_power;

                simple_XOrientation(velocity);
                Draw_Trajectory_Line();
            }
            if (isHolding && isReleased)
            {
                end_collider.gameObject.SetActive(true);
                RaycastHit2D ground_raycast = Physics2D.CircleCast(SlimeBody.transform.position, 0.43f, final_direction, length_of_line, wall_layer);

                if (ground_raycast.collider != null)
                {
                    line_end = ground_raycast.centroid;
                    trajectory_colliding = true;
                }


                //makes end collider be seen

                end_collider.position = line_end;
                if (hasJumped && !hasdoubleJumped)
                    hasdoubleJumped = true;

                SlimeBody.velocity = velocity;
                SlimeBody.gravityScale = 0;

                if (velocity == Vector2.zero)
                {
                    SlimeBody.gravityScale = slime_gravity;
                    jump_falling = true;
                }

                onGround = false;
                isSticked = false;
                hasJumped = true;
                isDragging = false;
                isReleased = false;
                isHolding = false;
                slimeAnimator.SetBool("isFallingTop", false);
                slimeAnimator.SetBool("isFallingSideways", false);

                toggle_trajectory_arrow(false);
            }
        }

        if (isSticked)
        {
            end_collider.gameObject.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        if (jump_falling)
        {
            Vector2 temp_velocity = SlimeBody.velocity;
            temp_velocity.x *= 1.0f - horizontal_drag;
            SlimeBody.velocity = temp_velocity;
        }
    }
    IEnumerator check_ifStuck()
    {
        if (hasJumped)
        {
            Vector3 current_pos = SlimeBody.transform.position;

            yield return new WaitForSeconds(0.2f);

            if (SlimeBody.transform.position == current_pos)
            {
                isSticked = true;
                onGround = true;
                hasJumped = false;

                slimeAnimator.SetBool("isIdle", true);
                slimeAnimator.SetBool("isSideways", false);
                slimeAnimator.SetBool("isFalling", false);

            }
        }
    }

    private void Draw_Trajectory_Line()
    {
        Vector3[] positions = new Vector3[2];

        positions[0] = Starting_Position;
        positions[1] = line_end;

        change_arrow_rotation();
        Trajectory_Line.SetPositions(positions);
    }

    internal void toggle_trajectory_arrow(bool booly)
    {
        try
        {
            Trajectory_Line.gameObject.SetActive(booly);
            trajectory_arrow.gameObject.SetActive(booly);
        }
        catch
        {

        }
    }

    private void change_arrow_rotation()
    {
        trajectory_arrow.position = line_end;

        var relativePos = line_end - (Vector2)Slime_Transform.position;
        var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
       
        trajectory_arrow.rotation = rotation;
    }

    protected void simple_XOrientation(Vector2 Direction)
    {
        if (isSticked && (onGround || fromTop) && !fromWalls )
        {
            if (Direction.x <= 0)
            {
                slimeSprite.flipX = true;
            }
            else
            {
                slimeSprite.flipX = false;
            }
        }
    }

    protected void simple_XOrientation(string Direction)
    {
        last_collision_direction = Direction;
        if (Direction == "Right")
        {
            slimeSprite.flipX = true;
        }
        else
        {
            slimeSprite.flipX = false;
        }
    }

    protected void simple_YOrientation(string Direction)
    {
        last_collision_direction = Direction;
        if (Direction == "Top")
        {
            slimeSprite.flipY = true;
        }
        else
        {
            slimeSprite.flipY = false;
        }
    }

    void PlayJumpEffect()
    {
        try
        {
            jumpEffect.Play();
        }
        catch{

        }
    }
}

