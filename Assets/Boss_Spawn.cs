using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spawn : MonoBehaviour
{
    public GameObject boss;
    [SerializeField]
    float bossSpeed;

    Animator bossAnimator;
    bool playerDied;
    bool bossSpawned = false;
    public GameObject normalBGMusic, bossBGMusic;
    [SerializeField]
    private string bossName;

    Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        playerDied = SlimeMovement.hasDied;
        bossAnimator = boss.gameObject.GetComponent<Animator>();
        startPos = boss.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (SlimeMovement.hasDied)
        {
            boss.transform.position = startPos;
            boss.SetActive(false);
            bossSpawned = false;
            normalBGMusic.SetActive(true);
            bossBGMusic.SetActive(false);
        }
        else
        {
          
            if (bossSpawned == true)
            {
                normalBGMusic.SetActive(false);
                bossBGMusic.SetActive(true);
                if (bossName == "golem")
                {

                }
                else
                {
                    MoveBoss();
                }
            }
        }
        
    }


    public void SpawnBoss() 
    {
        boss.SetActive(true);
        bossSpawned = true;
    } 

    public void MoveBoss()
    {
        boss.transform.Translate(Vector3.up * Time.deltaTime * bossSpeed, Space.World);

    }
}
