using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class SlimeDialogueManager : MonoBehaviour
{
    public Texture[] LilySprites;

    public RawImage LilyImage;

    public RawImage Dialogue_Box;

    public TextMeshProUGUI Dialogue_Text;

    private Queue<Dictionary<string, string>> Dialogue_Sequence;

    public Animator DialogueBoxAnimator,endCutsceneAnimator;

    public NextLevel next;


    public static bool in_Dialogue = false;
    public bool nextLevel,endWithCutscene;
    

    private bool still_typing = false;


    private Dictionary<string, string> received_dialogue;

    private float dialogue_cooldown = 0.5f;
    private float dialogue_timer = 0f;
    private bool can_skip_dialogue = false;

    

    public AudioSource Lily_Sound;

    //Controller
    private Gameplay controls;


    void Start()
    {
        Dialogue_Sequence = new Queue<Dictionary<string, string>>();
        Dialogue_Sequence.Clear();
        controls = new Gameplay();
        controls.PlayerControls.Jump.Enable();
        DialogueBoxAnimator = GameObject.Find("DialoguePanel").GetComponent<Animator>();
        Lily_Sound = GameObject.Find("Dialogue Sound").GetComponent<AudioSource>();
    }

    void Update()
    {
       
        if(/*Input.GetMouseButtonDown(0) && */controls.PlayerControls.Jump.triggered)
        {
            if (in_Dialogue && can_skip_dialogue)
            {
                StopAllCoroutines();
                Lily_Sound.Stop();
                if (Dialogue_Text.text == received_dialogue["Text"])
                {
                    still_typing = false;
                    DisplayNextData();
                }
                else
                {
                    Dialogue_Text.text = received_dialogue["Text"];
                    still_typing = false;
                    Lily_Sound.Stop();
                }
                dialogue_timer = 0f;
                can_skip_dialogue = false;
            }
        }

       
        if (in_Dialogue)
        {
            if (dialogue_timer < dialogue_cooldown)
            {
                dialogue_timer += Time.deltaTime;
                can_skip_dialogue = false;
            }
            else
            {
                can_skip_dialogue = true;
            }
        }
    }

    public void StartDialogue(SlimeDialogue dialogue)
    {
        in_Dialogue = true;

        DialogueBoxAnimator.SetBool("isOpen",true);
        foreach (Dictionary<string, string> Data in dialogue.Get_Dialogue_Data())
        {
            Dialogue_Sequence.Enqueue(Data);
        }
        Invoke("DisplayNextData",0.20f);
    }

    public void DisplayNextData()
    {
        DialogueBoxAnimator.SetTrigger("isTalking");
        
        if (Dialogue_Sequence.Count > 0 && !still_typing)
        {
            received_dialogue = Dialogue_Sequence.Dequeue();
           
               
            LilyImage.texture = LilySprites[GetLilySprite_Index(received_dialogue["Sprite"])];
            
            StopAllCoroutines();
            StartCoroutine(TypeSentence(received_dialogue["Text"]));
        }
        else if(Dialogue_Sequence.Count == 0 && !still_typing)
        {
            DialogueBoxAnimator.SetBool("isOpen", false);
            DialogueBoxAnimator.ResetTrigger("isTalking");
            in_Dialogue = false;
            //IDK if necessary with the endwithcutscene shizz ong
            if (nextLevel)
            {
                next.NextLevelChange();
            }
            if (endWithCutscene)
            {
                endCutsceneAnimator.SetTrigger("Play");
            }
            
        }

    }

    IEnumerator TypeSentence(string sentence)
    {
        still_typing = true;
        Dialogue_Text.text = "";
        foreach (char letter in sentence)
        {
            Dialogue_Text.text += letter;
            Lily_Sound.Play();
            yield return new WaitForSeconds(0.05f);
        }

        Lily_Sound.Stop();
        still_typing = false;
    }

    private int GetLilySprite_Index(string sprite_string)
    {
        if (sprite_string.Equals("lily_talk_defaultt"))
            return 0;
        else if (sprite_string.Equals("lily_talk_done"))
            return 1;
        else if (sprite_string.Equals("lily_talk_flustered"))
            return 2;
        else if (sprite_string.Equals("lily_talk_happy"))
            return 3;
        else if (sprite_string.Equals("lily_talk_scared"))
            return 4;
        else
            return 0;
    }
}
