using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Level_Manager : MonoBehaviour
{
    private Animator slime_level_animator;

    private Transform slime_level_transform;

    private Transform target_pos;

    private float move_speed = 5f;

    Transform[] all_level_button_pos;

    private List<Transform> move_sequence;

    private int slime_index_pos = 0;

    public NextLevel next_level_class;

    int button_pressed = 0;

    void Start()
    {
        slime_level_transform= GetComponent<Transform>();
        slime_level_animator = GetComponent<Animator>();

        move_sequence = new List<Transform>();

        GameObject[] gameobj_buttons = GameObject.FindGameObjectsWithTag("Level Buttons");

        all_level_button_pos = new Transform[gameobj_buttons.Length];

        for(int i = 0; i < gameobj_buttons.Length; i++)
            all_level_button_pos[i] = gameobj_buttons[i].GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (move_sequence.Count > 0)
            {
                target_pos = move_sequence[0];
                float step = move_speed* Time.deltaTime;
                slime_level_transform.position = Vector2.MoveTowards(slime_level_transform.position, target_pos.position, step);

                if (Vector2.Distance(slime_level_transform.position,target_pos.position) <= 0)
                {
                    move_sequence.RemoveAt(0);
                    slime_index_pos = get_button_index(target_pos);
                }
            }
            else
            {
                slime_level_animator.ResetTrigger("idle");
                slime_level_animator.SetTrigger("idle");
                slime_level_animator.ResetTrigger("moving");
            }
        }
        catch (ArgumentOutOfRangeException)
        {

        }
    }

    public void check_slime_on_button(int level_to_go)
    {
        try
        {
            if (slime_index_pos == button_pressed)
                next_level_class.LoadScreenScene(level_to_go);
        }
        catch (NullReferenceException)
        {

        }
    }

    private int get_button_index(Transform button_pos)
    {
        for (int i = 0; i < all_level_button_pos.Length; i++)
        {
            if (button_pos == all_level_button_pos[i])
                return i;
        }
        return 0;
    }

    public void slime_level_move(Transform button_pos)
    {
        button_pressed = get_button_index(button_pos);
        move_sequence.Clear();
        int target_index_pos = get_button_index(button_pos);

        if(slime_index_pos < target_index_pos)
        {
            for(int i = slime_index_pos+1; i < target_index_pos+1; i++)
            {
                move_sequence.Add(all_level_button_pos[i]);
            }
        }
        else if (slime_index_pos > target_index_pos)
        {
            for (int i = slime_index_pos -1; i > target_index_pos-1; i--)
            {
                move_sequence.Add(all_level_button_pos[i]);
            }
        }
        slime_level_animator.ResetTrigger("idle");
        slime_level_animator.ResetTrigger("moving");
        slime_level_animator.SetTrigger("moving");
    }
}
