using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss_Exhausted : StateMachineBehaviour
{

    public float exhaustionTimer;
    
    float exhaustTime,exhaustNow,maxExhaust;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isWeakened",false);
        
        if (maxExhaust <= exhaustionTimer)
        {
            exhaustTime = maxExhaust;
        }
        else if(maxExhaust == 0)
        {
            exhaustTime = exhaustionTimer;
        }
        maxExhaust = exhaustionTimer;
         
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (exhaustTime <= 0)
        {
            exhaustTime = 0;
            animator.SetBool("isExhausted", false);
        }
        else
        {
            exhaustTime -= Time.fixedDeltaTime;
            exhaustNow = exhaustTime;
            Debug.Log(exhaustTime + " real");
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GolemBoss_Movement.exhaustionMeter = 0;
        if (exhaustTime >= 0)
        {
            maxExhaust -= exhaustNow;
        }
        Debug.Log(maxExhaust);
    }


}
