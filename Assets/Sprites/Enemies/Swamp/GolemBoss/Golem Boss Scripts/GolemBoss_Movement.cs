using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss_Movement : StateMachineBehaviour
{


    Rigidbody2D player, bossGO;
    public static Vector2 playerPos;
    public float speed,cooldownTime;
    float attackTime = 0f;

    public static float exhaustionMeter;
    public float maxExhaustionMeter;
    


    float distanceBetween;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossGO = animator.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distanceBetween = (bossGO.transform.position - player.transform.position).magnitude;
        /*Debug.Log(exhaustionMeter);*/

        MovetoPlayer();
        
        if (exhaustionMeter > maxExhaustionMeter)
        {
           
            animator.SetBool("isWeakened", true);
            animator.SetBool("isExhausted", true);


        }
       else if (Time.time > attackTime)
        {
           
            if (distanceBetween <= 7f)
            {
                animator.SetTrigger("Smash");
                attackTime = Time.time + cooldownTime;
                exhaustionMeter += 30;

            }
            else
            {
                animator.SetTrigger("Throw");
                playerPos = player.transform.position;
                attackTime = Time.time + cooldownTime;
                exhaustionMeter += 20;
            }

        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distanceBetween = (bossGO.transform.position - player.transform.position).magnitude;
       

    }


    
    void MovetoPlayer()
    {

        Vector2 target = new Vector2(player.transform.position.x, bossGO.position.y);
        Vector2 newPos = Vector2.MoveTowards(bossGO.position, target, speed * Time.fixedDeltaTime);
        bossGO.MovePosition(newPos);
    }

}
