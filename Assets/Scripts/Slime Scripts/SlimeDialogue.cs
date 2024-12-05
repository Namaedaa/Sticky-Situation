using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlimeDialogue : MonoBehaviour
{
    [SerializeField]
    [TextArea(3,10)]
    public string[] dialogueText;
    public string[] dialogueSprite;
   

    public Dictionary<string, string>[] Dialogue_List;
  
    public Dictionary<string, string>[] Get_Dialogue_Data()
    {



        Dictionary<string, string>[] temp_list = new Dictionary<string, string>[dialogueText.Length]; 
        for (int i = 0; i < dialogueText.Length; i++) {


            
            Dictionary<string, string>[]  bruh = new Dictionary<string, string>[]
            {
                    new Dictionary<string, string>()
                {
                    {"Sprite", dialogueSprite[i]},
                    {"Text",dialogueText[i]}

                }
            };

            bruh.CopyTo(temp_list, i);
           

        }

        
        

        return temp_list;
    }
}
