using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBG : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private Renderer BgRenderer;
  
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        BgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
