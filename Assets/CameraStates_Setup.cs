using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStates_Setup : MonoBehaviour
{
    private CinemachineVirtualCamera SlimeCamera;

    private CinemachineVirtualCamera DeathCamera;

    void Start()
    {
        DeathCamera = gameObject.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        SlimeCamera = gameObject.transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();

        SlimeCamera.Follow = GameObject.Find("Slime").transform;
        DeathCamera.Follow = GameObject.Find("Death Area").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
