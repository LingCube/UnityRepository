using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletmove : MonoBehaviour
{
    public bool isright;
    public float force;
    public float time;
    public int ATK;
    // Start is called before the first frame update
    void Start()
    {
        if (isright == true)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0));
        }
        if (isright == false)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-force, 0));
        }
        Destroy(gameObject, time);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GRASSENEMY")
        {
            collision.gameObject.GetComponent<ENEMYHP>().Hp.gameObject.SetActive(true);
            collision.gameObject.GetComponent<ENEMYHP>().MaxHP.gameObject.SetActive(true);
            int GEdehp = ATK;
            collision.gameObject.GetComponent<ENEMYHP>().deHP(GEdehp);
        }
        if(collision.gameObject.tag=="ENEMY")
        {
            collision.gameObject.GetComponent<ENEMYHP>().Hp.gameObject.SetActive(true);
            collision.gameObject.GetComponent<ENEMYHP>().MaxHP.gameObject.SetActive(true);
            int Edehp = ATK - collision.gameObject.GetComponent<ENEMY>().DEF;
            if (Edehp < 0) Edehp = 0;
            collision.gameObject.GetComponent<ENEMYHP>().deHP(Edehp);
        }
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
