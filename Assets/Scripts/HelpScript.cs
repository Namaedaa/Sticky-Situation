using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpScript : MonoBehaviour
{

    public GameObject help;
    // Start is called before the first frame update

    private float textspeed = .05f;

    private TextMeshProUGUI current_textbox;

    private bool inDialogue = false;

    public int line_index;

    private string[] Lines = new string[]
    {
        @"Click and Drag in the opposite direction of slime to launch it",
        @"You can also stick to walls if you jump to it",
        @"Beware of Deadly Obstacles"
    };

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!inDialogue)
        {
            if (collision.tag == "Player")
            {
                inDialogue = true;
                help.SetActive(true);
                current_textbox = help.GetComponent<TextMeshProUGUI>();
                current_textbox.text = "";
                StartCoroutine("Dialogue_Routine");
            }
        }
    }

    IEnumerator Dialogue_Routine()
    {
        int index = 0;

        while (current_textbox.text != Lines[line_index])
        {
            current_textbox.text += Lines[line_index][index];
            index++;
            yield return new WaitForSeconds(textspeed);
        }

        /*yield return new WaitForSeconds(3f);

        print("Coroutine Finished");*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        help.SetActive(false);
        StopCoroutine("Dialogue_Routine");
        inDialogue = false;
    }
}
