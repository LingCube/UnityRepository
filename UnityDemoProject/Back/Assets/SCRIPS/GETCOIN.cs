using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GETCOIN : MonoBehaviour
{
    public Text coincountUI;
    public int coincount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="COIN")
        {
            Destroy(collision.gameObject);
            coincount += 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        coincountUI.text = coincount.ToString();
    }
}
