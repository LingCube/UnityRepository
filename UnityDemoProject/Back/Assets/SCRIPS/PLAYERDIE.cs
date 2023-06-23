using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYERDIE : MonoBehaviour
{
    public GameObject PLAYER;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            PLAYER.GetComponent<PLAYERHP>().HP = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
