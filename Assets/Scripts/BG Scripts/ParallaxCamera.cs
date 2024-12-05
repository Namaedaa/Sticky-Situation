using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement, float betaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    private float oldPositionX, oldPositionY;
    void Start()
    {
        oldPositionX = transform.position.x;
    }
    void FixedUpdate()
    {
        if (transform.position.x != oldPositionX || transform.position.y != oldPositionY)
        {
            if (onCameraTranslate != null)
            {
                float delta = oldPositionX - transform.position.x;
                float beta = oldPositionY - transform.position.y;
                onCameraTranslate(delta,beta);
            }
            oldPositionX = transform.position.x;
            oldPositionY = transform.position.y;
        }
    }
}
