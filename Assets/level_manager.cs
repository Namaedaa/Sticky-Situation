using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class level_manager : MonoBehaviour
{
    public Transform[] levels;

    public int level_start_index = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] temp_obj = GameObject.FindGameObjectsWithTag("Level Buttons");
        levels = new Transform[temp_obj.Length];

        int current_progress = PlayerPrefs.GetInt("Progress");

        for(int i = 0; i < temp_obj.Length; i++) 
        {
            levels[i] = temp_obj[i].GetComponent<Transform>();
            if(current_progress < (level_start_index + i))
            {
                levels[i].GetComponent<Image>().color = new Color(0.39f,0.39f,0.39f);
                levels[i].GetComponent<Button>().interactable = false;
                levels[i].GetChild(0).gameObject.SetActive(true);
                if(i == temp_obj.Length - 1)
                {
                    GameObject.Find("base").GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
