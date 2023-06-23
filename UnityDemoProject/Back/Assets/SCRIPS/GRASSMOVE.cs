using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GRASSMOVE : MonoBehaviour
{
    public Transform up;
    public Transform down;
    public float speed;
    public bool isup = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(isup)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            if (transform.position.y>=up.position.y)
            {
                isup = false;
            }
        }
        else
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            if (transform.position.y<=down.position.y)
            {
                
                isup = true;
            }
        }
    }
}
