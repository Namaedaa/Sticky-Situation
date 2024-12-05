using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class NextLevel : MonoBehaviour
{
    Animator loadingScreenAnimator;
    public GameObject loadingScreen;

    private int Save_index;
    
    void Start()
    {
        loadingScreenAnimator = loadingScreen.GetComponent<Animator>(); 
    }

    void Update()
    {
    }

    public void NextLevelChange()
    {
        loadingScreen.SetActive(true);
        loadingScreenAnimator.SetBool("Play", true);
        StartCoroutine(LoadSceneAsync());
        
    }
    

    public void LoadScreenScene(int index)
    {
        loadingScreen.SetActive(true);
        loadingScreenAnimator.SetBool("Play", true);
        StartCoroutine(LoadSpecific_screen(index));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Save_index = SceneManager.GetActiveScene().buildIndex + 1;
            save_progress(Save_index);
            NextLevelChange();
        }
    }

    private void save_progress(int save_index)
    {
        if(save_index != 0)
        {
            PlayerPrefs.SetInt("Progress", save_index);
        }
    }
    IEnumerator LoadSceneAsync()
    {
        PauseManager.game_paused = false;
        SlimeDialogueManager.in_Dialogue = false;
        Time.timeScale = 1.0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        
        operation.allowSceneActivation = false;

        yield return new WaitForSeconds(4f);
        loadingScreenAnimator.SetBool("Play", false);
        operation.allowSceneActivation = true;
        
    }

    IEnumerator LoadSpecific_screen(int index)
    {
        PauseManager.game_paused = false;
        SlimeDialogueManager.in_Dialogue = false;
        Time.timeScale = 1.0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(4f);
        operation.allowSceneActivation = true;
        loadingScreenAnimator.SetBool("Play", false);

    }
}
