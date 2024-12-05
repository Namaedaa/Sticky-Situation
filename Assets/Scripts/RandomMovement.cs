using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class RandomMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Trajectory_Dots;

    public GameObject[] Dots;

    public GameObject DotSprite;

    private Rigidbody2D SlimeBody;

    private CircleCollider2D SlimeCollider;

    private Animator slimeAnimator;

    private SpriteRenderer slimeSprite;

    private GameObject RespawnBox;

    private GameObject DeathBox;

    private CinemachineVirtualCamera DeathCamera;

    public Animator CameraStates;

    

    private float secondsElapsed = 0f;

    private bool isSticked = false;

    private bool onGround = false;

    private bool hasJumped = false;

    private bool fromTop = false;

    private bool fromWalls = false;

    private bool hasDied = false;


    //Trajectory

    private bool isDragging = false;

    public float DotShift = 1f;

    private Vector2 Starting_Position;

    private Vector2 Ending_Position;

    private Vector2 Trajectory_End;

    private Vector2 Launch_Velocity;

    private Vector2 Initial_Position;

    public float Launch_power = 0.082f;

    public AudioSource JumpSfx;

    public AudioSource BumpSfx;

    public AudioSource DeathSfx;


    void Start()
    {
        RespawnBox = GameObject.Find("Respawn Area");
        DeathBox = GameObject.Find("Death Area");
        DeathCamera = GameObject.Find("Death Camera").GetComponent<CinemachineVirtualCamera>();

        SlimeBody = GetComponent<Rigidbody2D>();
        SlimeCollider = GetComponent<CircleCollider2D>();

        slimeAnimator = GetComponent<Animator>();
        slimeSprite = GetComponent<SpriteRenderer>();
        Dots = new GameObject[9];

        for(int i = 0; i < 9; i++)
        {
            Dots[i] = GameObject.Find("Dots" + (i+1));
            /*Dots[i].GetComponent<SpriteRenderer>().sprite = */
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        StartCoroutine("check_ifStuck");

        if (!hasDied)
        {
            if (!hasJumped)
            {
                bool isClosetoClick = isCloseToClick();

                //Inital start on dragging
                if (Input.GetMouseButtonDown(0) && isClosetoClick)
                {
                    isDragging = true;
                    Starting_Position = Input.mousePosition;
                    
                    Trajectory_Dots.SetActive(true);
                }
                //Constant call on dragging
                if (Input.GetKey(KeyCode.Mouse0) && isDragging)
                {
                    MoveTrajectoryDots();
                }

                //Drag Release
                if (Input.GetMouseButtonUp(0) && isDragging)
                {
                    SlimeBody.gravityScale = 1f;

                    Ending_Position = Trajectory_End;

                    Launch_Slime();


                    onGround = false;
                    isSticked = false;
                    hasJumped = true;
                    isDragging = false;
                    slimeAnimator.SetBool("isFallingTop", false);
                    slimeAnimator.SetBool("isFallingSideways", false);



                    Trajectory_Dots.SetActive(false);

                    secondsElapsed = 0f;
                }
            }
        }
        

        if (isSticked)
        {
            hasJumped = false;
            if (!onGround)
            {
                secondsElapsed += Time.deltaTime;

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
                    SlimeBody.gravityScale = 1;
                    secondsElapsed = 0;

                    isSticked = false;
                }
                else if (secondsElapsed >= 3)
                {
                    if (fromTop)
                    {
                        slimeAnimator.SetBool("isFallingTop", true);
                        slimeAnimator.SetBool("isIdle", false);
                        slimeAnimator.SetBool("isSideways", false); 
                    }
                    if (fromWalls)
                    {
                        slimeAnimator.SetBool("isFallingSideways", true);
                        slimeAnimator.SetBool("isIdle", false);
                        slimeAnimator.SetBool("isSideways", false);
                    }
                }
                
            }
        }

        if (!isSticked && !onGround)
        {
            hasJumped = true;
            slimeAnimator.SetBool("isFalling", true);
            slimeAnimator.SetBool("isIdle", false);
            slimeAnimator.SetBool("isSideways", false);
            SlimeBody.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }

        if (!isDragging)
            Trajectory_Dots.SetActive(false);
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
                onGround = false;
                hasJumped = false;

                slimeAnimator.SetBool("isIdle", true);
                slimeAnimator.SetBool("isSideways", false);
                slimeAnimator.SetBool("isFalling", false);

            }
        }
    }

    private void Launch_Slime()
    {

        Vector2 direction = Starting_Position - Ending_Position;

        SlimeBody.velocity = direction * Launch_power;

        if (direction.x > 0)
            simple_XOrientation("Left");
        else if (direction.x < 0)
            simple_XOrientation("Right");

        JumpSfx.Play();
    }

    

    private bool isCloseToClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float mouse_slime_distance = Vector2.Distance(SlimeBody.transform.position, mousePos);

        if (mouse_slime_distance < 2.0f)
            return true;
        else
            return false;
    }

    private void MoveTrajectoryDots()
    {
        Vector2 Current_pos = Input.mousePosition;

        Vector2 direction = Current_pos - Starting_Position; //direction from Center to Cursor

        Trajectory_End = Starting_Position+  Vector2.ClampMagnitude(direction, 200f);

        Launch_Velocity = Starting_Position - Trajectory_End;

        Initial_Position = SlimeBody.transform.position;

        for (int i = 0 ; i < 9; i++)
        {
            Dots[i].transform.position = CalculatePosition(DotShift * i);
        }
    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        return Physics2D.gravity * elapsedTime * elapsedTime * 0.02f +
                   Launch_Velocity / 60 * elapsedTime + Initial_Position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasDied)
        {
            secondsElapsed = 0f;
            BumpSfx.Play();

            Vector2 contactPoint = collision.GetContact(0).normal;

            if (collision.collider.tag == "Spikes")
            {
                DeathBox.transform.position = SlimeBody.transform.position;
                DeathCamera.Follow = DeathBox.transform;

                CameraStates.SetBool("isDead", true);
                SlimeCollider.isTrigger = true;
                DeathSfx.Play();

                slimeAnimator.SetBool("isDead", true);
                SlimeBody.velocity = new Vector2(0, 0);
                slimeAnimator.SetBool("isSideways", false);
                slimeAnimator.SetBool("isFalling", false);
                

                hasDied = true;

                Invoke("Respawn", 2f);
            }
            else if (contactPoint.x >= -1 && contactPoint.x <= -0.7)
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
                SlimeBody.gravityScale = 1;
                SlimeBody.velocity = new Vector2(0, 0);
                slimeAnimator.SetBool("isIdle", true);
                slimeAnimator.SetBool("isSideways", false);
                slimeAnimator.SetBool("isFalling", false);

                fromTop = false;
                fromWalls = false;
                isSticked = true;
                onGround = true;
            }


            /*Debug.Log("X is " + contactPoint.x);
            Debug.Log("Y is " + contactPoint.y);*/

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void Respawn()
    {
        SlimeBody.velocity = new Vector2(0, 0);

        slimeAnimator.SetBool("isIdle", false);
        slimeAnimator.SetBool("isDead", false);
        slimeAnimator.SetBool("isSideways", false);
        slimeAnimator.SetBool("isFalling", true);


        hasDied = false;

        fromTop = false;
        fromWalls = false;
        isSticked = false;
        onGround = false;

        SlimeBody.transform.position = RespawnBox.transform.position;
        CameraStates.SetBool("isDead", false);
        SlimeCollider.isTrigger = false;

        DeathCamera.Follow = this.transform;
    }

    private void simple_XOrientation(string Direction)
    {
        if(Direction == "Right")
        {
            slimeSprite.flipX = true;
        }
        else
        {
            slimeSprite.flipX = false;
        }
    }

    private void simple_YOrientation(string Direction)
    {
        if (Direction == "Top")
        {
            slimeSprite.flipY = true;
        }
        else
        {
            slimeSprite.flipY = false;
        }
    }

    public void TopFall()
    {
        Debug.Log("falling now from top");
        TopFallingNow();
    }
    private void TopFallingNow()
    {
        Trajectory_Dots.SetActive(false);
        SlimeBody.gravityScale = 1;
        secondsElapsed = 0;
        slimeAnimator.SetBool("isFalling", true);
        slimeAnimator.SetBool("isFallingTop", false);
        slimeAnimator.SetBool("isIdle", false);
        slimeAnimator.SetBool("isSideways", false);
        isSticked = false;
    }

    public void SidewaysFall()
    {
        Debug.Log("falling now from sideways");
        SidewaysFallingNow();
    }
    private void SidewaysFallingNow()
    {
        Trajectory_Dots.SetActive(false);
        SlimeBody.gravityScale = 1;
        secondsElapsed = 0;
        slimeAnimator.SetBool("isFalling", true);
        slimeAnimator.SetBool("isFallingSideways", false);
        slimeAnimator.SetBool("isIdle", false);
        slimeAnimator.SetBool("isSideways", false);
        isSticked = false;
    }
}
