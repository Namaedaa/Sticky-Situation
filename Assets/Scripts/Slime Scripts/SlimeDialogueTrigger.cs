using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeDialogueTrigger : MonoBehaviour
{
    public SlimeDialogue slimedialogue;
    public SlimeDialogueManager dialogueManager;
    Collider2D collider;
    [SerializeField]
    bool hasCutscene;
    [SerializeField]
    bool cutsceneOnly;
    [SerializeField]
    bool repeatableCutscene;




    Animator CutsceneAnimator;
    

    public void Start()
    {
        collider = this.gameObject.GetComponent<Collider2D>();
        collider.enabled = true;
        if (hasCutscene == true)
        {
            CutsceneAnimator = this.gameObject.GetComponent<Animator>();
        }
    }
    
    void Update()
    {
        if (repeatableCutscene && SlimeMovement.hasDied)
        {
            CutsceneAnimator.ResetTrigger("Play");
            collider.enabled = true;
        }
       
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<SlimeDialogueManager>().StartDialogue(slimedialogue);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player"){


           
            collider.enabled = false;
           
           
            if (cutsceneOnly == true)
            {
                if (hasCutscene == true)
                {
                    CutsceneAnimator.SetTrigger("Play");
                }
            }
            else
            {
                TriggerDialogue();
                if (hasCutscene == true)
                {
                    CutsceneAnimator.SetTrigger("Play");
                }
            }
           
            
            
        }
    }
   
    

    public void EndGameObject()
    {
        this.gameObject.SetActive(false);
    }
}
