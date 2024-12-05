using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackToMenu : MonoBehaviour
{
    public NextLevel next_class;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        next_class.LoadScreenScene(0);
    }
}
