using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss_Orientation : MonoBehaviour
{
    Transform playerPos,bossPos;


    public GameObject rockPrefab,CRShairRockPrefab;
    
    Transform rockSpawnerThrow;
    public GameObject[] Spikes;
    public int MaxProjectiles;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        bossPos = this.gameObject.GetComponent<Transform>();
        rockSpawnerThrow = GameObject.Find("Rock Spawner").transform;
       

    }

    // Update is called once per frame
    void Update()
    {
        ChangeOrientation();
    }
    void ChangeOrientation()
    {
        if (bossPos.position.x <= playerPos.position.x )
        {
            bossPos.rotation = new Quaternion(0,180,0,0);
        }
        else
        {
            bossPos.rotation = new Quaternion(0, 0, 0, 0);
        }

    }

    public void SpawnProjectile()
    {

        for (int i = 0; i < MaxProjectiles; i++)
        {

            Instantiate(rockPrefab, rockSpawnerThrow.position, rockSpawnerThrow.rotation);


        }
    }

   

    public void SpawnSpikes()
    {
        for(int i = 0; i < Spikes.Length; i++)
        {
            Spikes[i].SetActive(true);
        }
       
    }
    
}
