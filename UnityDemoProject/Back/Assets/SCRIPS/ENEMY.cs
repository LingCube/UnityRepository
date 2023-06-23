using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY : MonoBehaviour
{
    public GameObject PlayerUI;
    public Transform left;
    public Transform right;
    public float speed;
    public int ATK;
    public int DEF;
    public bool ismove = true;
    public bool isright = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void enemymove()
    {
        if (isright==false)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= left.position.x)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                isright = true;
            }
        }
        else
        {
            transform.Translate(Vector2.right * -speed * Time.deltaTime);
            if (transform.position.x >= right.position.x)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                isright = false ;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(ismove)
        {
            enemymove();
        }
    }
}
