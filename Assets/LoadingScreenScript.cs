using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenScript : MonoBehaviour
{
    int loopTimes = 0;
    Animator loadingScreenAnimator;
    // Start is called before the first frame update
    void Start()
    {
        loadingScreenAnimator = this.gameObject.GetComponent<Animator>();
    }
    public void AnimLoopers()
    {
        loopTimes += 1;
    }
    public void AnimationChecker()
    {
        Debug.Log(loopTimes);
        if (loopTimes == 15)
        {
            loadingScreenAnimator.SetBool("Play", false);
        }
        
        else
        {
            loadingScreenAnimator.SetBool("Play", true);
            AnimLoopers();
        }
       

    }
}
