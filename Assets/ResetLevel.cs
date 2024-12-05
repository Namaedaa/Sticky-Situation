using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    private bool respawned = false;

    private GameObject[] spiders;

    private Transform[] pos;

    void Start()
    {
        spiders = GameObject.FindGameObjectsWithTag("Enemy");
        pos = new Transform[spiders.Length];

        for(int i = 0; i < spiders.Length; i++)
        {
            pos[i] = spiders[i].transform;
        }

        foreach(GameObject spider in spiders)
        {
            Debug.Log(spider.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SlimeMovement.hasDied)
        {
            if (!respawned)
                StartCoroutine("Respawn_Enemy");
            else
                StopCoroutine("Respawn_Enemy");
        }
    }

    IEnumerator Respawn_Enemy()
    {
        
        for (int i = 0; i < spiders.Length; i++)
        {
            Instantiate(spiders[i], pos[i].position, pos[i].rotation);
        }
        respawned = true;
        yield return null;
    }
}
