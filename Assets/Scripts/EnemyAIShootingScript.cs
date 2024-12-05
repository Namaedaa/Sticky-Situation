using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIShootingScript : MonoBehaviour
{
    [field: SerializeField]
    public bool PlayerDetected { get; private set; }
    public Vector2 DirectionToTarget => target.transform.position - detectorOrigin.position;

    [Header("OverlapBox parameters")]
    public Transform detectorOrigin;
    public Vector2 detectorSize = Vector2.one;
    public Vector2 detectorOriginOffset = Vector2.zero;

    public float detectionDelay = 0.3f;

    public LayerMask detectorLayerMask;

    [Header("Gizmo parameters")]
    public Color gizmoIdleColor = Color.green;
    public Color gizmoDetectedColor = Color.red;
    public bool showGizmos = true;

    public GameObject target;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    bool isFiring = true;
    public float bulletSpeed = 10;
    public bool alreadyFired = false;
    public float rateOfFire= 1.5f;

    SpriteRenderer BulletSprite;

    Animator enemyAnimator;
    public float speed;
    public bool MoveRight;

    bool isDead = false;
    public GameObject enemyDeathFX;
    AudioSource deathSfx;

    public int fire_counter;
    public int max_fire_counter;
    public int wait_after_maxfire;

    //Health Scripts
    private SpriteRenderer enemySpriteRenderer;

    public GameObject Target
    {
        get => target;
        private set
        {
            target = value;
            PlayerDetected = target != null;
        }
    }

    public void Start()
    {
        detectorOrigin = this.gameObject.transform;
        StartCoroutine(DetectionCoroutine());
        enemyAnimator = this.GetComponent<Animator>();
        deathSfx = this.GetComponent<AudioSource>();
        BulletSprite = bulletPrefab.GetComponent<SpriteRenderer>();
        
        enemySpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (!isDead)
        {
            if (MoveRight)
            {
                transform.Translate( Time.deltaTime * speed, 0, 0);
                if (!isFiring)
                    transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.Translate( Time.deltaTime * -speed, 0, 0);
                if (!isFiring)
                    transform.localScale = new Vector2(1, 1);
            }
        }
        else
        {
            speed = 0;
        }
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        PerformDetection();
        StartCoroutine(DetectionCoroutine());
    }

    IEnumerator BulletFire()
    {
        while (true)
        {
            if (fire_counter < max_fire_counter)
            {
                yield return new WaitForSeconds(rateOfFire);
                enemyAnimator.SetBool("enemyWalking", false);
                enemyAnimator.SetTrigger("enemyAttack2");
                enemyAnimator.SetBool("enemyIdle", false);
            }
            else
            {
                Invoke("Reset_bullets", 1.5f);
                break;
            }
        }
    }
    public void BulletShoot()
    {
        enemyAnimator.SetBool("enemyWalking", false);
        enemyAnimator.SetBool("enemyIdle", false);
        
        if (isFiring && fire_counter < max_fire_counter)
        {
            Debug.Log("Spider fire counter: " + fire_counter);
            fire_counter++;
            //MoveRight
            if (detectorOrigin.transform.position.x < target.transform.position.x)
            {
                BulletSprite.flipX = true;
                detectorOrigin.transform.localScale = new Vector2(-1, 1);
                var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.right * bulletSpeed;
            }
            //!MoveRight
            else
            {

                BulletSprite.flipX = false;
                detectorOrigin.transform.localScale = new Vector2(1, 1);
                var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = -bulletSpawnPoint.right * bulletSpeed;
            }
        }
    }

    private void Reset_bullets()
    {
        fire_counter = 0;
        StartCoroutine("BulletFire");
    }

    public void PerformDetection()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize, 0, detectorLayerMask);
        if (collider != null)
        {
            enemyAnimator.SetBool("enemyWalking", false);
            
            enemyAnimator.SetBool("enemyIdle", true);
            Target = collider.gameObject;
            if (!alreadyFired)
            {
                isFiring = true;
                StartCoroutine("BulletFire");
                
                speed = 0;
                alreadyFired = true;
            }

        }
        else
        {
            fire_counter = 0;
            enemyAnimator.ResetTrigger("enemyAttack2");
            enemyAnimator.SetBool("enemyWalking", true);
            enemyAnimator.SetBool("enemyIdle", false);
            Target = null;
            StopCoroutine("BulletFire");
            speed = 2;
            isFiring = false;
            alreadyFired = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos && detectorOrigin != null)
        {
            Gizmos.color = gizmoIdleColor;
            if (PlayerDetected)
            {
                Gizmos.color = gizmoDetectedColor;
            }
            Gizmos.DrawCube((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize);
        }

    }
    

    public void DeathAnimation()
    {
        isDead = true;
        enemyAnimator.SetBool("enemyWalking", false);
        enemyAnimator.SetBool("enemyAttack2", false);
        enemyAnimator.SetBool("enemyIdle", false);
        StopCoroutine("BulletFire");
        deathSfx.Play();
        enemyDeathFX.SetActive(true);
        enemyAnimator.SetBool("enemyDied", true);
    }
    public void isAttacked()
    {
        enemySpriteRenderer.color = Color.red;
    }
    public void EnemyDie()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("turn"))
        {
            MoveRight = !MoveRight;
        }
    }
}
