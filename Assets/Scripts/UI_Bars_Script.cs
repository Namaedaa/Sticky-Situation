using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bars_Script : MonoBehaviour
{
    // HP and MP Variables

    private Slider MP_Bar;

    private float secondsElapsed = 0;

    void Start()
    {
        MP_Bar = GameObject.Find("MP_Border").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        secondsElapsed = SlimeMovement.secondsElapsed;

        UpdateMP_Bar();
    }

    void UpdateMP_Bar()
    {
        MP_Bar.value = Mathf.Abs(5 - secondsElapsed);
        if(MP_Bar.value <= 0)
        {
            MP_Bar.value = 0;
        }
    }
}
