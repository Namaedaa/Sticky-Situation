using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

public class MainMenuNext : MonoBehaviour
{
    public GameObject MainMenu;

    public GameObject PlayMenu;

    public GameObject OptionsMenu;

    public NextLevel next_level_class;

    public Button con_button;

    public Image con_renderer;

    public Toggle Hard_mode_toggle;

    // IT173P

    /*public TMP_InputField player_name_field;

    public Transform leaderboard_parent;

    public GameObject row_prefab;

    IDbConnection dbcon;

    IDbCommand dbcomm;*/
    

    void Start()
    {

        var temp_color = con_renderer.color;
        if (PlayerPrefs.GetInt("Progress",0) == 0)
        {
            con_button.interactable = false;
            temp_color.a = 0.5f;
            con_renderer.color = temp_color;
        }
        else
        {
            con_button.interactable = true;
            temp_color.a = 1f;
            con_renderer.color = temp_color;
        }

        // IT173P
       /* if (PlayerPrefs.GetString("Player_Name","") != null)
        {
            player_name_field.text = PlayerPrefs.GetString("Player_Name","");
        }

        load_leaderboard();*/
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu_Handler(-1);
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /*private void load_leaderboard()
    {
        //Insert SQLlite code here that gets data from database
        create_database();

        dbcomm = dbcon.CreateCommand();
        string q_get_scores = "SELECT * FROM sticky_situation_leaderboard_tbl ORDER BY time ASC";

        dbcomm.CommandText = q_get_scores;
        IDataReader reader = dbcomm.ExecuteReader();

        int rank = 0;

        while (reader.Read())
        {
            *//*Debug.Log("Data 0: " + reader[0].ToString() + "Data 1: " + reader[1].ToString());*//*

            rank++;

            var rowObject = Instantiate(row_prefab);
            rowObject.transform.parent = leaderboard_parent;
            rowObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rank.ToString();
            rowObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =  reader[0].ToString();
            rowObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =  reader[1].ToString();
        }
    }*/

   /* private void create_database()
    {
        // Create database
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "StickyS_DB";

        // Open connection
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
        // Create table
        dbcomm = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS sticky_situation_leaderboard_tbl (name VARCHAR, time NUMERIC )";

        dbcomm.CommandText = q_createTable;
        dbcomm.ExecuteReader();
    }*/

    public void MainMenu_Handler(int index)
    {
        if(Hard_mode_toggle.isOn)
            SlimeAnimation.isHardMode = true;
        else if(!Hard_mode_toggle.isOn)
            SlimeAnimation.isHardMode = false;

/*
        // IT173P
        PlayerPrefs.SetString("Player_Name", player_name_field.text.Trim());*/

        if(index == -1)
        {
            MainMenu.SetActive(true);
            PlayMenu.SetActive(false);
            OptionsMenu.SetActive(false);
        }
        if (index == 0)
        {
            //Start Button
            MainMenu.SetActive(false);
            PlayMenu.SetActive(true);
        }
        if(index == 1)
        {
            //Options Button
            OptionsMenu.SetActive(true);
            MainMenu.SetActive(false);
        }
        else if(index == 2)
        {
            //Credits Button
            next_level_class.LoadScreenScene(11);
        }
        else if(index == 3)
        {
            //New Game Button
            PlayerPrefs.SetInt("Progress", 2);
            next_level_class.NextLevelChange();


            //IT173P
            PlayerPrefs.SetFloat("Level1-0", 0);
            PlayerPrefs.SetFloat("Level1-1", 0);
            PlayerPrefs.SetFloat("Level1-2", 0);
            PlayerPrefs.SetFloat("Level1-3", 0);
        }
        else if (index == 4)
        {
            //Continue Game Button
            int saved_progress = PlayerPrefs.GetInt("Progress", 2);
            if (saved_progress != 0)
            {
                next_level_class.LoadScreenScene(saved_progress);
            }
        }
    }

    public void load_progress()
    {
        int saved_progress = PlayerPrefs.GetInt("Progress",0);
        if(saved_progress != 0)
        {
            next_level_class.LoadScreenScene(saved_progress);
        }
    }
}
