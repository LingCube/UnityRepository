using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GRASSMOVE1 : MonoBehaviour
{
    public Transform left;
    public Transform right;
    public Transform player;
    public float speed;
    public bool isright = true;
    public bool playerleft = false;
    public bool playerright = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            if (transform.localScale == new Vector3(1, 1, 1))
            {
                playerleft = true;
                playerright = false;
            }
            else  
            {
                if(transform.localScale == new Vector3(-1, 1, 1))
                {
                    playerright = true;
                    playerleft = false;

                }
                
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerleft = false;
            playerright = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isright)
        {
            if (playerleft==true) player.Translate(Vector2.right * speed * Time.deltaTime);
            else playerleft = false;
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            transform.localScale = new Vector3(1, 1, 1);
            if (transform.position.x >= right.position.x) isright = false;
        }
        else
        {
            if (playerright==true) player.Translate(Vector2.left * speed * Time.deltaTime);
            else playerleft = false;
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            transform.localScale = new Vector3(-1, 1, 1);
            if (transform.position.x <= left.position.x) isright = true;
        }
    }
}
