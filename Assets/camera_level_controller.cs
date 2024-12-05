using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class camera_level_controller : MonoBehaviour
{
    private CinemachineVirtualCamera[] all_level_cameras;

    private Collider2D slime_collider;
    void Start()
    {
        all_level_cameras = GameObject.FindGameObjectsWithTag("Camera Level").Select
            (x => x.GetComponent<CinemachineVirtualCamera>()).ToArray();

        slime_collider = GameObject.Find("Slime").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeCamera()
    {

    }

}
