using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss_Throw : StateMachineBehaviour
{
    public GameObject CRSHRockPrefab;
    Transform playerPos;
    public static Transform LastCRSHrockPos; 
    public float CRSHspeed;
    GameObject CRSHclone;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CRSHclone = Instantiate(CRSHRockPrefab, animator.transform.position,new Quaternion(0,0,0,0));
        LastCRSHrockPos = CRSHclone.transform;
        
        
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        
       
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.length<=4f) 
        {
            CRSHclone.transform.position = Vector2.Lerp(CRSHclone.transform.position, playerPos.position, CRSHspeed * Time.fixedDeltaTime);    
        }
        /*else
        {
            LastCRSHrockPos = CRSHclone.transform;
        }*/


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LastCRSHrockPos = CRSHclone.transform;
        Destroy(CRSHclone);
    }


}
