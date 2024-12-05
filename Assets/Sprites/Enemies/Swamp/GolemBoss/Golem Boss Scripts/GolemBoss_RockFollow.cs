using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss_RockFollow : MonoBehaviour
{
    // Start is called before the first frame update

    
    Vector2 playerPos,rockPos;
    Animator rockAnimator;
    public float rockSpeed;
    float distanceBetween;
   /* SlimeMovement slimeMovement;
*/
     void Awake()
    {
        
    }

    void Start()
    {
       
        try
        {
            playerPos = GameObject.Find("Trajectory Arrow").transform.position;
        }
        catch
        {
            playerPos = GolemBoss_Throw.LastCRSHrockPos.position;
        }
       
        rockAnimator = gameObject.GetComponent<Animator>();
        /*slimeMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<SlimeMovement>();*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /* playerPos = slimeMovement.Slime_Transform.position;*/
        rockPos = this.gameObject.transform.position;
        distanceBetween = (rockPos - playerPos).magnitude;
        if (distanceBetween == 0)
        {
            rockAnimator.SetTrigger("isBreak");
        }
        else
        {
            gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, playerPos, rockSpeed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "EnemyProjectile" && collision.gameObject.tag != "Wall" )
        {
            rockAnimator.SetTrigger("isBreak");
        }

    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }


}
