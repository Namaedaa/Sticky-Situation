using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject White_bg;

    public GameObject Pause_button;

    public Animator Resume_Animator;

    private AudioSource lily_sound;

    public GameObject resumeButton;

    public static bool game_paused = false;


   
    private void Start()
    {

        lily_sound = GameObject.Find("Dialogue Sound").GetComponent<AudioSource>();
       
    }

  



   



    public void pause_game()
    {


        game_paused = true;
        White_bg.SetActive(true);
        Resume_Animator.SetTrigger("Paused");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);

        Time.timeScale = 0;
        lily_sound.volume = 0;

        Pause_button.SetActive(false);
    }

    public void resume_game()
    {
        game_paused = false;
        EventSystem.current.SetSelectedGameObject(null);
        Resume_Animator.SetTrigger("Resumed");
        lily_sound.volume = 0.7f;
    }

    public void remove_white_bg()
    {
        Pause_button.SetActive(true);
        Time.timeScale = 1;
        White_bg.SetActive(false);
    }
}
