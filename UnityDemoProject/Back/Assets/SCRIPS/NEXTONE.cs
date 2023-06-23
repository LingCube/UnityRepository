using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEXTONE : MonoBehaviour
{
    public GameObject player;
    public GameObject inputF;
    public GameObject nextonetrue;
    public GameObject nextonefalse;
    public Transform nextone;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void sure()
    {
        nextonetrue.SetActive(false);
        player.GetComponent<PLAYER>().ismove = true;
        player.transform.position = nextone.position;
    }
    public void not()
    {
        nextonetrue.SetActive(false);
        player.GetComponent<PLAYER>().ismove = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            inputF.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            inputF.SetActive(false);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(inputF.activeSelf==true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if(player.GetComponent<GETCOIN>().coincount >= 33)
                {
                    nextonetrue.SetActive(true);
                    player.GetComponent<PLAYER>().ismove = false;
                }
                if (player.GetComponent<GETCOIN>().coincount < 33)
                {
                    player.GetComponent<PLAYER>().ismove = false;
                    nextonefalse.SetActive(true);
                    inputF.SetActive(false);
                }
            }
        }
        if(inputF.activeSelf==false)
        {
            if(nextonefalse.activeSelf==true)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    nextonefalse.SetActive(false);
                    player.GetComponent<PLAYER>().ismove = true;
                }
            }
        }
    }
}
